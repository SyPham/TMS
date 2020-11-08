/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TokenService } from './token.service.service';

describe('Service: Token.service', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TokenService]
    });
  });

  it('should ...', inject([TokenService], (service: TokenService) => {
    expect(service).toBeTruthy();
  }));
});
