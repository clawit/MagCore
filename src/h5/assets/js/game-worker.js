importScripts('databus.js');

onmessage =function (evt){
    var game = evt.data;//通过evt.data获得发送来的数据
    // console.log('game worker msg received.');
    // console.log(evt);
    while(true){
        loadGame(game.Id);

        sleep(400);
    }
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