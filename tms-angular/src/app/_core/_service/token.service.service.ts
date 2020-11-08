import { Injectable } from '@angular/core';
const ACCESS_TOKEN = 'token';
const REFRESH_TOKEN = 'refresh_token';
@Injectable({
  providedIn: 'root'
})
export class TokenService {

constructor() { }
getToken(): string { return localStorage.getItem('token'); }
getRefreshToken(): string { return localStorage.getItem('refresh_token'); }

saveRefreshToken(refreshToken: string): void { return localStorage.setItem('refresh_token', refreshToken); }
saveToken(token: string): void { return localStorage.setItem('token', token); }

removeRefreshToken(): void { return localStorage.removeItem('refresh_token'); }
removeToken(): void { return localStorage.removeItem('token'); }

}
