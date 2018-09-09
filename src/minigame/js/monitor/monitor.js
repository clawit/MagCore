import Sprite from '../base/sprite'
import Map from '../monitor/map.js'
import DataBus from '../databus'

// 相关常量设置
var worker = wx.createWorker('workers/request/index.js') // 文件名指定 worker 的入口文件路径，绝对路径

const screenWidth = window.innerWidth
const screenHeight = window.innerHeight

let databus = new DataBus()

export default class Monitor extends Sprite {
  constructor() {
    super();
    self = this;
    console.log('Monitor created.');

    //TODO: transfer gcode to gid
    // 以下语句模拟最后的返回情况, 要完成以上TODO
    databus.gid = '1db2d61999b64da0aa41a72f928e58b4';

    //load map
    wx.request({
      url: databus.baseUrl + 'api/game/' + databus.gid,
      method: 'GET',
      success: function (response) {
        console.log('game response recevied:');
        console.log(response);
        var mapName = response.data.Map;
        self.map = new Map(mapName);
      }
    })


    worker.postMessage({
      key: 'hello worker'
    })
  }

  

  update() {
    //console.log('Monitor updated.');
    
  }

  
  render(ctx) {
    //console.log('Monitor redered.');
    //console.log(this.map)
    if (this.map != undefined){
      this.map.render(ctx);
    }
  }
}