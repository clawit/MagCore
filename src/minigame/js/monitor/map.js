import Sprite from '../base/sprite'
import DataBus from '../databus'

let databus = new DataBus()

export default class Map extends Sprite {
  constructor(map) {
    super();

    console.log('loading map info:' + map);
    wx.request({
      url: databus.baseUrl + 'api/map/' + map,
      method: 'GET',
      success: function (response) {
        console.log('map response recevied:');
        console.log(response);

      }
    })
  }

  render(ctx) {
    //console.log('map redered.');

  }
}