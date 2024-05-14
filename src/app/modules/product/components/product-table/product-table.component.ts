import { Component } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { SingalrService } from '../../services/singalr.service';

@Component({
  selector: 'app-product-table',
  templateUrl: './product-table.component.html',
  styleUrls: ['./product-table.component.css']
})
export class ProductTableComponent {
  // Names of columns:
  displayedColumns: string[] = [
  'productId', 
  'name', 'description', 'stock',
  'price','subscribe','Unsubscribe'];

  // List oF my Groups:
  myProductSubscribtion : string[] = [];

  // User name from token:
  userName = "BARAABA";

  constructor(public productService: ProductService,public singlarService:SingalrService) {
   
  // Get Products:  
  this.getProducts();

  // Start Connection to SignalR:
  this.singlarService.startConnection();

  // Get My Groups:
  this.singlarService.getMyGroup(this.userName).subscribe({
    next : (data:any) => {
      if (data.length != 0) {
        data.forEach((item:any) => {;
          this.myProductSubscribtion.push(item.groupName);
        });
        
        // Re Join To My Groups (When reload pages):
        setTimeout(() => {
          if (this.myProductSubscribtion && this.myProductSubscribtion.length != 0) {
            this.myProductSubscribtion!.forEach((productId:any) => {
              this.singlarService.subscribeToProduct(productId);
            });
          }
          // Update UnReading Messages:
          this.singlarService.updateUnreadMsgStatus();
        }, 1000);
      }
    }
  });
  
  
}
readedMsg = [];
UnReadedMsg : any[] = [];
allMsg : any[] = [];


delay(ms: number) {
  return new Promise( resolve => setTimeout(resolve, ms) );
}
getProducts()
{
  this.productService.get()
}

// When user click to subscribe to product:
subscribeToProduct(productId:string)
{
  this.myProductSubscribtion.push(productId);
  this.singlarService.subscribeToProduct(productId);
}

// When user click to subscribe to product:
unSubscribeToProduct(productId:string)
{
  this.myProductSubscribtion = this.myProductSubscribtion.filter(prod => prod != productId);
  this.singlarService.unSubscribeToProduct(productId);
}

getUnReadedMsg(){
  this.singlarService.getUnReadedMsg(this.userName).subscribe({
    next : (data:any) => {
      data.forEach((item:any) => {
        this.UnReadedMsg.push(item.message);
      });
    }
  })
}

getReadedMsg(){
  this.singlarService.getReadedMsg(this.userName).subscribe({
    next : (data:any) => this.readedMsg = data
  })
}

makeMsgAsReading(){
  this.singlarService.makeMsgAsReading(this.userName).subscribe({
    next : (data:any) => {
      this.singlarService.getAllMsg(this.userName).subscribe({
        next  : (data:any) => {
            this.allMsg = data;
        }
      })
    }
  })
}
}
