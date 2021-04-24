import { HttpClient } from '@angular/common/http';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private loading = new BehaviorSubject<boolean>(false);
  private loading$ = this.loading.asObservable();


  constructor(private http: HttpClient) { }

  public getLoading() {
    return this.loading$;
  }

  public setLoading(val: boolean) {
    this.loading.next(val);
  }

  public searchLocations(value) {
    if (!value) return of([]);
    let params = {
      searchedVal: value
    };
    return this.http.get<any[]>('api/Weather/Search', { params: params });
  }

  public getCurrentWeather(locationKey) {
    if (!locationKey) return of(null);

    let params = {
      locationKey: locationKey
    };

    return this.http.get<any>('api/weather/GetCurrentWeather', { params: params });
  }

  public getFavorites() {

    return this.http.get<any[]>('api/favorites/GetFavorites');
  }

  public addToFavorites(locationKey, localizedName) {
    if (!locationKey) return of(false);

    let params = {
      locationKey: locationKey,
      localizedName: localizedName
    };
    return this.http.get<any>('api/favorites/AddToFavorites', { params: params });
  }


  public deleteFromFavorites(locationKey) {
    if (!locationKey) return of(false);

    let params = {
      locationKey: locationKey
    };
    return this.http.get<any>('api/favorites/DeleteFavorite', { params: params });
  }

}
