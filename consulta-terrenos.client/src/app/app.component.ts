import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';

import { faMap, faHouse, faRightFromBracket, faRightToBracket, faGlobe, faStar, faChartSimple } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  faMap = faMap;
  faStar = faStar;
  faGlobe = faGlobe;
  faHouse = faHouse;
  faLogin = faRightToBracket;
  faLogout = faRightFromBracket;
  faChartSimple = faChartSimple;
  constructor(public authService: AuthService) { }
}
