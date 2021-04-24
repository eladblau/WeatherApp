import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, SimpleChanges, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-weather-display',
  templateUrl: './weather-display.component.html',
  styleUrls: ['./weather-display.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WeatherDisplayComponent implements OnInit, OnDestroy {

  @Input("locationsList") locationsList: any[] = [];
  @Output() favoriteChanged: EventEmitter<any> = new EventEmitter();

  selectedLocation: any;
  subWeather: any;
  loadingResults: boolean;
  currentWeather: any;
  watherImageSrc: string;
  subFavoriteAdd: any;
  subFavoriteRemove: any;

  constructor(private apiService: ApiService,
    private _snackBar: MatSnackBar) { }


  ngOnDestroy(): void {
    this.subWeather?.unsubscribe();
    this.subFavoriteAdd?.unsubscribe();
    this.subFavoriteRemove?.unsubscribe();
  }


  ngOnChanges(changes: SimpleChanges) {
    if (changes.locationsList) {
      this.currentWeather = null;
      this.selectedLocation = null;
    }
  }

  ngOnInit(): void {
  }




  locationClicked(event, loc) {
    if (this.selectedLocation == loc) return;
    //Get current weather
    this.selectedLocation = loc;
    this.getLocationWeather(loc);
  }

  private getLocationWeather(location) {
    this.loadingResults = true;
    this.currentWeather = null;
    this.subWeather = this.apiService.getCurrentWeather(location?.key).subscribe(res => {
      this.loadingResults = false;
      this.currentWeather = res;
      let iconNumber = res?.weatherIcon;
      if (iconNumber?.toString().length == 1) {
        iconNumber = '0' + iconNumber;
      }
      this.watherImageSrc = `assets/weather_icons/${iconNumber}-s.png`;
    });
  }


  addRemoveFavoriteClicked($event) {
    if (this.currentWeather?.isFavorite) {
      //Remove from favorites
      this.subFavoriteRemove = this.apiService.deleteFromFavorites(this.selectedLocation?.key)
        .subscribe(res => {
          if (res) {
            this.currentWeather.isFavorite = false;
            this._snackBar.open('Removed from favorites', null, { duration: 3000 });
            this.favoriteChanged.emit(this.selectedLocation);
          }
        });
    }
    else {
      //Add to favorites
      this.subFavoriteAdd = this.apiService.addToFavorites(this.selectedLocation?.key, this.selectedLocation?.localizedName)
        .subscribe(res => {
          if (res) {
            this.currentWeather.isFavorite = true;
            this._snackBar.open('Added to favorites', null, { duration: 3000 });
            this.favoriteChanged.emit(this.selectedLocation);
          }
        });
    }
  }


}
