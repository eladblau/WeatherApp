import { Component, OnDestroy, OnInit } from '@angular/core';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.scss']
})
export class FavoritesComponent implements OnInit, OnDestroy {

  locationsList = [];
  subFavorites: any;

  constructor(private apiService: ApiService) { }


  ngOnDestroy(): void {
    this.subFavorites?.unsubscribe();
  }

  ngOnInit(): void {
    this.getFavorites();
  }

  private getFavorites() {
    this.apiService.setLoading(true);
    this.subFavorites = this.apiService.getFavorites().subscribe(res => {
      this.apiService.setLoading(false);
      this.locationsList = res;
    });
  }


  favoriteChanged(event) {
    //Refresh the list
    this.getFavorites();
  }

}
