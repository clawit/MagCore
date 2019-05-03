var databus = {
    imgUrl: 'http://magcore.clawit.com/assets/images/',
    apiUrl: 'http://api.magcore.clawit.com/',
    //apiUrl: 'http://localhost:8000/',
    mapApi: undefined,
    gameApi: undefined,

    inited: false,

    game: undefined,
    players: new Array(),

    server: 'api',

    icons: new Array()
};

databus.mapApi = databus.apiUrl + 'api/map/';
databus.gameApi = databus.apiUrl + 'api/game/';

databus.icons[0] = databus.imgUrl + 'Banana.png';
databus.icons[1] = databus.imgUrl + 'Cherry.png';
databus.icons[2] = databus.imgUrl + 'Grapes.png';
databus.icons[3] = databus.imgUrl + 'GreenMelon.png';
databus.icons[4] = databus.imgUrl + 'Lemon.png';
databus.icons[5] = databus.imgUrl + 'Mulberry.png';
databus.icons[6] = databus.imgUrl + 'Pear.png';
databus.icons[7] = databus.imgUrl + 'Pineapple.png';
databus.icons[8] = databus.imgUrl + 'Radish.png';
databus.icons[9] = databus.imgUrl + 'Watermelon.png';
