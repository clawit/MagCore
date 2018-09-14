/*
  worker msg 格式
  {
    "action": "detail",
    "gid": "1A2ECB678CBDDDF90DFB1A2ECB678CBDDD"
    "data": {}
  }
 */

const utils = require('./utils')

function sleep(duration) {
  var now = new Date().getTime();
  while (new Date().getTime() < now + duration) { /* do nothing */ }
}

worker.onMessage(function(msg){
  //console.log('worker msg received:');
  //console.log(msg);
  onProcess(msg);
})

function loadUrl(url) {
  //console.log('loading url');
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function () {
    if (xhttp.readyState == 4 && xhttp.status == 200) {
      //console.log('Worker load src back');
      //console.log(xhttp.response);

      worker.postMessage({
        action: 'detail',
        data: xhttp.response
      })

      sleep(500);
    }
  };
  //console.log(url);
  xhttp.open("GET", url, false);
  xhttp.send();
}

function onProcess(msg){
  var action = msg.action;
  console.log(msg.url);
  if( msg.action == 'detail'){
    // wx.request({
    //   url: msg.url,
    //   method: 'GET',
    //   success: function (response) {
    //     console.log('game response recevied:');
    //     console.log(response);

    //     worker.postMessage({
    //       action: 'detail',
    //       data: response
    //     })
    //   }
    // })

    while(true){
      loadUrl(msg.url);
      
    }
    

    

  }

}
