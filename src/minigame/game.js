import './js/libs/weapp-adapter'
import './js/libs/symbol'

import Main from './js/main'

wx.getSetting({
  success(res) {
    if (!res.authSetting['scope.record']) {
      wx.authorize({
        scope: 'scope.userInfo',
        success() {
          // 用户已经同意小程序使用录音功能，后续调用 wx.startRecord 接口不会弹窗询问
          wx.startRecord()
        }
      })
    }
  }
})


// var launch = wx.getLaunchOptionsSync()
// console.log(launch.query.scene)
// 以上代码等效于下方代码

wx.onShow(function (option) {
  var scene_args = option.query.scene;
  console.log('option.query.scene:' + scene_args);

  if (scene_args != undefined && scene_args.length > 2){
    let scene_arg1 = scene_args.substr(0, 2); //截0-2字符
    let scene_arg2 = scene_args.substring(2); //从第2位开始截字符
    console.log('scene_arg1:' + scene_arg1);
    console.log('scene_arg2:' + scene_arg2);
    if (scene_arg1 == 'm-' && scene_arg2 != ''){
      new Main(scene_arg2);
      return;
    }
  }

  new Main();
})

