
$(function() {
    FastClick.attach(document.body);
});

$(function() {
    //获取查询字符串,读取gid
    var queries = {};
    $.each(document.location.search.substr(1).split('&'),function(c,q){
        var i = q.split('=');
        queries[i[0]] = i[1];
    });
    console.log(queries);

    //$.showLoading();



});

var OnError = function(rr) {
    console.log('Error');

    $.hideLoading();

    $.toast("创建失败", 'cancel');
}

var loadMap = function(game) {
    let map = game.Map;
    //清理
    $('#map').empty();

    //获取map
    $.ajax({
        url: databus.mapApi + map,
        type: "GET",
        dataType: 'json',
        success: function(data) {
            console.log('get map success');
            console.log(data);
            
            //计算li节点的大小
            var widthLi = $('#map').width() / data.Rows[0].length;

            //创建节点
            for (let i = 0; i < data.Rows.length; i++) {
                const row = data.Rows[i];
                //create ul
                var $ul=$('<ul></ul>');
                //create cells
                for (let j = 0; j < row.length; j++) {
                    const cell = row[j];
                    let idLi = 'li-' + j + '-' +i;
                    let idImg = 'img-' + j + '-' +i;
                    let $li = $('<li id="' + idLi + '"></li>');

                    let $img = $('<img id="' + idImg + '" />');
                    $li.append($img);
                    $ul.append($li);

                    //判断cell的类型
                    var bgImg = 'url(../assets/images/Rect.png)';
                    switch (cell) {
                        case '0':
                            bgImg = 'url(../assets/images/Empty.png)';
                            break;
                        case '1':
                            bgImg = 'url(../assets/images/Rect.png)';
                            break;
                        case '2':
                            bgImg = 'url(../assets/images/Base.png)';
                            break;
                        default:
                            break;
                    }

                    //设置li节点的css
                    $li.css({ 
                        "width": widthLi, 
                        "height": widthLi,
                        "background-size": widthLi,
                        "background-image": bgImg
                   });
                }
                //
                $('#map').append($ul);
            }
    
            //开始重绘线程
            if(databus.inited == false){
                startWorker(game);
                databus.inited = true;
            }

            //隐藏loading
            $.hideLoading();
        },
        error: OnError
    });


    

}

var loadGame = function(gid) {
    $.ajax({
        url: databus.gameApi + gid,
        type: "GET",
        dataType: 'json',
        success: function(data) {
            console.log('get game success');
            console.log(data);

            if(data.Map == undefined){
                $.hideLoading();
                $.toast("无法定位游戏", 'cancel');
                return;
            }

            databus.game = data;

            //首次加载游戏时要先加载地图
            if(databus.inited == false){
                loadMap(data);
            }

        },
        error: OnError
    });
}

var update = function(game){
    //游戏状态改变
    let state = '游戏状态: ';
    if(game.State != databus.game.State || $('#descState').html() == '' ){
        switch (game.State) {
            case 0:
                state += '等待中';
                break;
            case 1:
                state += '游戏开始';
                break;
            case 2:
                state += '已结束';
                break;
            case 3:
                state += '已销毁';
                break;
            default:
                break;
        }

        $('#descState').html(state);
    }

    //更新到databus
    databus.game = game;
    
    if(game.State != undefined && game.State < 2
        && (databus.players.length - 1) != game.Players.length ) {
        if(databus.players.length == 0 && game.Players.length == 0)
            return;
        //更新players信息
        databus.players = new Array();
        for (var i = 0; i < game.Players.length; i++){
          var p = game.Players[i];
          databus.players[p.Index] = p;
        }

        //
        let content = '当前参赛: ' + game.Players.length + '人';
        
        $('#descPlayers').html(content);
    }
}

var render = function(){
    let game = databus.game;
    if(game.State != undefined && game.State < 3) {
        for (let i = 0; i < game.Cells.length; i++) {
            const row = game.Cells[i];
            
            for (let j = 0; j < row.length; j++) {
                const cell = row[j];
                
                let idLi = 'li-' + j + '-' +i;
                let idImg = 'img-' + j + '-' +i;
                let $li = $('#' + idLi);
                let $img = $('#' + idImg);

                //设置基地
                if(cell.Type == 2 && $li != undefined ) {
                    $li.css({ 
                        "background-image": "url(../assets/images/Base.png)"
                   });
                }

                //设置玩家占领
                if(cell.State > 0 && $img != undefined) {
                    let owner = cell.Owner;
                    let player = databus.players[owner];
                    let color = player.Color;
                    let icon = databus.icons[color];

                    if(cell.State == 1) {
                        let src = $img.attr('src');
                        //console.log('cell.State == 1');
                        //console.log(src);
                        if(src != undefined && src.indexOf('_blink.png') < 0){
                            icon = icon.replace('.png', '_blink.png');

                            //console.log('icon replaced.');
                            //console.log(icon);
                        }
                    }
                    
                    $img.attr('src', icon) 
                }
            }
        }
    }
    
}