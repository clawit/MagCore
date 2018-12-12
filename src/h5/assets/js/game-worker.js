importScripts('databus.js');

onmessage =function (evt){
    var game = evt.data;//通过evt.data获得发送来的数据
    // console.log('game worker msg received.');
    // console.log(evt);

    setServer(game.Server);

    while(true){
        loadGame(game.Id);

        sleep(400);
    }
  }

  var setServer = function(svr){
    databus.server = svr;
    if(svr == 'dev') {
        databus.apiUrl = 'http://dev.magcore.clawit.com/';
    }
    else if (svr == 'test') {
        databus.apiUrl = 'http://test.magcore.clawit.com/';
    }
    databus.mapApi = databus.apiUrl + 'api/map/';
    databus.gameApi = databus.apiUrl + 'api/game/';
}

var loadGame = function(gid) {
    //console.log('loading game');
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (xhttp.readyState == 4 && xhttp.status == 200) {
            //console.log('Worker load src back');
            //console.log(xhttp);

            postMessage(xhttp.response);
        }
        else if(xhttp.readyState == 4 && xhttp.status >= 300){
            postMessage('stopWorker()');
        }
    };
    //console.log(gid);
    xhttp.open("GET", databus.gameApi + gid, false);
    xhttp.send();
}

function sleep(duration) {
    var now = new Date().getTime();
    while (new Date().getTime() < now + duration) { /* do nothing */ }
}