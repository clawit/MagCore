var worker = undefined;

function startWorker(game) {
    if(typeof(Worker) !== "undefined") {
        if(typeof(worker) == "undefined") {
            worker = new Worker("../assets/js/game-worker.js");
            worker.postMessage(game);
        }
        worker.onmessage = function(event) {
            //console.log('Worker msg recevied');
            //console.log(event.data);

            if(event.data == 'stopWorker()') {
                stopWorker();
                return;
            }

            let game = JSON.parse(event.data);
            
            if(databus.game.Status >= 2) {
                stopWorker();
            }

            update(game);
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