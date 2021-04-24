import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { of } from 'rxjs';
import { startWith, debounceTime, switchMap, map } from 'rxjs/operators';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-weather',
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.scss']
})
export class WeatherComponent implements OnInit, OnDestroy {


  searchInputFormControl = new FormControl();
  locationsList = [];
  loadingResults: boolean;
  dataObs: any;
  currentWeather: any;
  subWeather: any;
  selectedLocation: any;
  watherImageSrc = '';
  subFavorite: any;

  constructor(private apiService: ApiService,
    private _snackBar: MatSnackBar) { }

  ngOnDestroy(): void {
    this.subWeather?.unsubscribe();
    this.subFavorite?.unsubscribe();
  }

  ngOnInit(): void {

    this.dataObs = this.searchInputFormControl.valueChanges
      .pipe(
        startWith(''),
        debounceTime(500),
        switchMap((value) => {
          if (value?.length > 2) {
            return this.searchLocations(value);
          }
          else {
            return of(null);
          }
        })
      ).subscribe(res => res);

  }


  private searchLocations(value) {
    this.apiService.setLoading(true);
    return this.apiService.searchLocations(value)
      .pipe(
        map(data => {
          this.apiService.setLoading(false);
          this.locationsList = data;
          return data;
        })
      );
  }



  clearInput(event) {
    this.searchInputFormControl.reset();
  }

}
