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

    //transfer databus.gcode to gid
    wx.request({
      url: databus.baseUrl + 'api/wxgame/' + databus.gcode + '/id',
      method: 'GET',
      success: function (response) {
        console.log('wxgame response recevied:');
        console.log(response);
        if (response.statusCode == 400) {
          wx.showToast({ title: response.data });
          
          return;
        }
        else{
          databus.gid = response.data.toLowerCase();

          //load game
          self.game = new Game(databus.gid);
          //console.log(self.game);
          if (self.game == undefined || self.game.error != undefined) {
            return;
          }

        }

      }
    })

    

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