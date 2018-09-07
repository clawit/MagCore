import './js/libs/weapp-adapter'
import './js/libs/symbol'

import Main from './js/main'

// var launch = wx.getLaunchOptionsSync()
// console.log(launch.query.scene)
// 以上代码等效于下方代码

wx.onShow(function (option) {
  console.log('option.query.scene:' + option.query.scene);
  let scene_args = option.query.scene;
  
  if (scene_args != undefined && scene_args.length > 8){
    let scene_arg1 = scene_args.substr(0, 7);
    let scene_arg2 = scene_args.substring(8);
    console.log('scene_arg1:' + scene_arg1);
    console.log('scene_arg2:' + scene_arg2);
    if (scene_arg1 == 'monitor' && scene_arg2 != ''){
      new Main(scene_arg2);
      return;
    }
  }

  new Main();
})

