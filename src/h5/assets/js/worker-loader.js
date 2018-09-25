var w;

function startWorker() {
    if(typeof(Worker) !== "undefined") {
        if(typeof(w) == "undefined") {
            w = new Worker("game-worker.js");
        }
        w.onmessage = function(event) {
            console.log('Worker msg recevied');
            console.log(event.data);
        };
    } else {
        document.getElementById("result").innerHTML = "Sorry! No Web Worker support.";
    }
}

function stopWorker() { 
    w.terminate();
    w = undefined;
}