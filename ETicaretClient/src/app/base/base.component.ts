import { NgxSpinnerService } from "ngx-spinner";



export class BaseComponent {
  constructor(private spinner: NgxSpinnerService) { }

  showSpinner(type: SpinnerTypes = SpinnerTypes.Ball8Bits) {
    this.spinner.show(type, { color: '#3F51B5' });
  }

  hideSpinner(type: SpinnerTypes = SpinnerTypes.Ball8Bits) {
    this.spinner.hide(type);
  }

  transitionSpinner(type: SpinnerTypes = SpinnerTypes.Ball8Bits) {
    this.spinner.show(type)
    setTimeout(() => {
      this.spinner.hide(type)
    }, 750);
  }
}

export enum SpinnerTypes {
  Ball8Bits = "spinner_1",
  BallClipRotateMultiple = "spinner_2",
  Cog = "spinner_3"
}
