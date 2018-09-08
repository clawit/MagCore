import Sprite from '../base/sprite'
import DataBus from '../databus'

// 相关常量设置
const screenWidth = window.innerWidth
const screenHeight = window.innerHeight

let databus = new DataBus()

export default class Monitor extends Sprite {
  constructor() {
    super();

    console.log('Monitor created.');
    
  }

  update() {
    console.log('Monitor updated.');
    
  }

  
  render(ctx) {
    console.log('Monitor redered.');
  }
}