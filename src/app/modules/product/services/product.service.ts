import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ProductService {
  public products: any[] = [];
  private url = "https://localhost:7132/api/Product"
  constructor(private http: HttpClient) { }

   public get():any[] { 
    this.http.get(this.url).subscribe((data:any)=>{
      this.products=data as any[];
      
      return this.products;
     });
     return this.products;
    } 
}
