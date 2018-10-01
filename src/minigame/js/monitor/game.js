import Sprite from '../base/sprite'
import Map from '../monitor/map.js'
import DataBus from '../databus'

const RECT_IMG_SRC = 'images/Rect.png'
const BASE_IMG_SRC = 'images/Base.png'
const EMPTY_IMG_SRC = 'images/Empty.png'

const IMG_0 = 'images/Banana.png'
const IMG_1 = 'images/Cherry.png'
const IMG_2 = 'images/Grapes.png'
const IMG_3 = 'images/GreenMelon.png'
const IMG_4 = 'images/Lemon.png'
const IMG_5 = 'images/Mulberry.png'
const IMG_6 = 'images/Pear.png'
const IMG_7 = 'images/Pineapple.png'
const IMG_8 = 'images/Radish.png'
const IMG_9 = 'images/Watermelon.png'

const screenWidth = window.innerWidth
const screenHeight = window.innerHeight
const IMG_WIDTH = 32
const IMG_HEIGHT = 32

let databus = new DataBus()

var worker = wx.createWorker('workers/request/index.js') // 文件名指定 worker 的入口文件路径，绝对路径
worker.onMessage(function (msg) {
  
  databus.game = JSON.parse(msg.data);

  //console.log('worker msg data received at main thread');
  console.log(databus.game);
})

export default class Game extends Sprite {
  constructor(id) {
    super();

    self = this;
    this.imgRect = new Image();
    this.imgRect.src = RECT_IMG_SRC;
    this.imgBase = new Image();
    this.imgBase.src = BASE_IMG_SRC;
    this.imgEmpty = new Image();
    this.imgEmpty.src = EMPTY_IMG_SRC;

    this.img0 = new Image();
    this.img0.src = IMG_0;
    this.img1 = new Image();
    this.img1.src = IMG_1;
    this.img2 = new Image();
    this.img2.src = IMG_2;
    this.img3 = new Image();
    this.img3.src = IMG_3;
    this.img4 = new Image();
    this.img4.src = IMG_4;
    this.img5 = new Image();
    this.img5.src = IMG_5;
    this.img6 = new Image();
    this.img6.src = IMG_6;
    this.img7 = new Image();
    this.img7.src = IMG_7;
    this.img8 = new Image();
    this.img8.src = IMG_8;
    this.img9 = new Image();
    this.img9.src = IMG_9;

    this.gameId = id;

    this.error = undefined;

    console.log('loading game info:' + id);
    //load game
    wx.request({
      url: databus.baseUrl + 'api/game/' + id,
      method: 'GET',
      success: function (response) {
        console.log('game response recevied:');
        console.log(response);
        if (response.data.Map == undefined){
          wx.showToast({title: '无法定位该游戏'});
          self.error = '无法定位该游戏';
          return;
        }
        var mapName = response.data.Map;
        self.map = new Map(mapName);

        worker.postMessage({
          action: 'detail',
          url: databus.baseUrl + 'api/game/' + id
        })
      }
    })

  }

  update(){
    if (databus.game != undefined) {
      //如果游戏状态是0 (等待中)
      if(databus.game.State >= 0) {
        //每次都要更新玩家
        //console.log(databus.game.Players);
        databus.players = new Array();
        for (var i = 0; i < databus.game.Players.length; i++){
          var p = databus.game.Players[i];
          databus.players[p.Index] = p;
        }
        //console.log(databus.players);
      }

    }
  }

  render(ctx) {
    if( this.map != undefined){
      this.map.render(ctx);
    }

    if( databus.game != undefined){
      //console.log(databus.game)

      //如果游戏状态是1 (游戏中)
      if(databus.game.State >= 0){
        this.renderCells(ctx);

      }
    }
    

  }

  renderCells(ctx){
    for (var i=0; i < databus.game.Cells.length; i++){
      var row = databus.game.Cells[i];
      for (var j = 0; j < row.length; j++) {
        var cell = row[j];

        //如果类型是2(基地)
        if(cell.Type == 2){
          //console.log(i);
          //console.log(j);
          //console.log(cell);
          //需要覆盖一下此cell, 突出基地
          ctx.drawImage(
            this.imgBase,
            0,
            0,
            IMG_WIDTH,
            IMG_HEIGHT,
            5 +
            (j * this.map.edgeLength),
            (screenHeight - (databus.game.Cells.length * this.map.edgeLength)) / 2.0 +
            (i * this.map.edgeLength),
            this.map.edgeLength,
            this.map.edgeLength
          )
        }

        var img = undefined;
        //单元格中玩家状态绘制(1=闪烁,2=被占领)
        if(cell.State >= 1){
          var owner = cell.Owner;
          var player = databus.players[owner];
          var color = player.Color;
          //console.log(color)
          switch(color){
            case 0:
              img = this.img0;
              break;
            case 1:
              img = this.img1;
              break;
            case 2:
              img = this.img2;
              break;
            case 3:
              img = this.img3;
              break;
            case 4:
              img = this.img4;
              break;
            case 5:
              img = this.img5;
              break;
            case 6:
              img = this.img6;
              break;
            case 7:
              img = this.img7;
              break;
            case 8:
              img = this.img8;
              break;
            case 9:
              img = this.img9;
              break;
            default:
              break;
          }
          //console.log(databus.frame);
          if(cell.State == 1){
            if(databus.frame > 30)
              return;
          }
          
          ctx.drawImage(
            img,
            0,
            0,
            IMG_WIDTH,
            IMG_HEIGHT,
            5 +
            (j * this.map.edgeLength),
            (screenHeight - (databus.game.Cells.length * this.map.edgeLength)) / 2.0 +
            (i * this.map.edgeLength),
            this.map.edgeLength,
            this.map.edgeLength
          )

          

        }
      }
    }
  }
}