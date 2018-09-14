import Sprite from '../base/sprite'
import Game from '../monitor/game.js'
import DataBus from '../databus'

// 相关常量设置
const screenWidth = window.innerWidth
const screenHeight = window.innerHeight

let databus = new DataBus()

export default class Monitor extends Sprite {
  constructor() {
    super();
    self = this;
    console.log('Monitor created.');

    wx.showLoading({
      title: '加载游戏中...',
      mask: true
    })

    //TODO: transfer gcode to gid
    // 以下语句模拟最后的返回情况, 要完成以上TODO
    databus.gid = '9116bdc745ac4030a3ba7644aad9f080';

    //load game
    this.game = new Game(databus.gid);

  }

  

  update() {
    //console.log('Monitor updated.');
    if (this.game != undefined) {
      this.game.update();
    }
  }

  
  render(ctx) {
    //console.log('Monitor redered.');
    //console.log(this.game)
    //console.log(this.game.map)

    if (this.game != undefined){
      this.game.render(ctx);
    }
  }
}