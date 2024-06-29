import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
// import { AuthService } from './auth.service'; 

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  // constructor(private authService: AuthService, private router: Router) {}
  constructor(private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const userId = route.params['userId'];
    // const loggedInUserId = this.authService.getUserId();

    // if (userId && loggedInUserId && userId === loggedInUserId) {
    if (userId) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
