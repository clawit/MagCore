var worker = undefined;

function startWorker(game) {
    if(typeof(Worker) !== "undefined") {
        if(typeof(worker) == "undefined") {
            worker = new Worker("http://monitor.magcore.clawit.com/assets/js/game-worker.js");
            worker.postMessage(game);
        }
        worker.onmessage = function(event) {
            //console.log('Worker msg recevied');
            //console.log(event.data);

            if(event.data == 'stopWorker()') {
                stopWorker();
            }
            else {
                databus.game = JSON.parse(event.data);
            }
            
            if(databus.game.Status >= 2) {
                stopWorker();
            }

            update();
            render();
        };
    } else {
        $.toast("浏览器不支持", 'cancel');
    }
}

function stopWorker() { 
    console.log('stopping worker...');
    worker.terminate();
    worker = undefined;
}