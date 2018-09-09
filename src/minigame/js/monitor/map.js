import Sprite from '../base/sprite'
import DataBus from '../databus'

const RECT_IMG_SRC = 'images/Rect.png'
const BASE_IMG_SRC = 'images/Base.png'
const EMPTY_IMG_SRC = 'images/Empty.png'

const screenWidth = window.innerWidth
const screenHeight = window.innerHeight
const IMG_WIDTH = 32
const IMG_HEIGHT = 32

let databus = new DataBus()

export default class Map extends Sprite {
  constructor(map) {
    super();

    self = this;
    this.imgRect = new Image();
    this.imgRect.src = RECT_IMG_SRC;
    this.imgBase = new Image();
    this.imgBase.src = BASE_IMG_SRC;
    this.imgEmpty = new Image();
    this.imgEmpty.src = EMPTY_IMG_SRC;

    console.log('loading map info:' + map);
    wx.request({
      url: databus.baseUrl + 'api/map/' + map,
      method: 'GET',
      success: function (response) {
        console.log('map response recevied:');
        console.log(response);
        self.rows = response.data.Rows;
        self.edgeLength = 1.0 * (screenWidth - 10) / self.rows[0].length;
      }
    })
  }

  render(ctx) {
    //console.log('map redered.');
    if (this.rows != undefined) {
      for (var i = 0; i < this.rows.length; i++) {
        var row = this.rows[i];
        for (var j = 0; j < row.length; j++){
          var cell = row[j];

          ctx.drawImage(
            this.imgRect,
            0,
            0,
            IMG_WIDTH,
            IMG_HEIGHT,
            5 + 
            (j * this.edgeLength),
            (screenHeight - (this.rows.length * this.edgeLength)) / 2.0 + 
            (i * this.edgeLength),
            this.edgeLength,
            this.edgeLength
          )
        }
      }
    }


  }
}