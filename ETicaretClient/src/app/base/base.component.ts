import { NgxSpinnerService } from "ngx-spinner";



export class BaseComponent {
  constructor(private spinner: NgxSpinnerService) { }

  showSpinner(type: SpinnerTypes = SpinnerTypes.Ball8Bits) {
    this.spinner.show(type, { color: '#3F51B5' });
    setTimeout(() => {
      this.spinner.hide(type);
    }, 1000);
  }
}

export enum SpinnerTypes {
  Ball8Bits = "spinner_1",
  BallClipRotateMultiple = "spinner_2",
  Cog = "spinner_3"
}
