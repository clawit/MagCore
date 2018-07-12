## 创建游戏

### URL
ht<span></span>tp://106.75.33.221:6000/api/game

### Method
POST

### Header
Content-Type: application/json

### Body
```json
{
	"Map": "RectSmall"
}
```

支持的地图列表:
- 小地图: RectSmall
- 中型地图: RectMid
- 大型地图: RectLarge

### 返回结果
Status Code: 200

Content: 新创建游戏的GameId

### 示例
#### Python
```python
import requests

url = "http://106.75.33.221:6000/api/game"

payload = "{\"Map\":\"RectSmall\"}"
headers = {
    'Content-Type': "application/json",
    'Cache-Control': "no-cache"
    }

response = requests.request("POST", url, data=payload, headers=headers)

print(response.text)
```

#### JavaScript
```javascript
var settings = {
  "async": true,
  "crossDomain": true,
  "url": "http://106.75.33.221:6000/api/game",
  "method": "POST",
  "headers": {
    "Content-Type": "application/json",
    "Cache-Control": "no-cache"
  },
  "processData": false,
  "data": "{\"Map\":\"RectSmall\"}"
}

$.ajax(settings).done(function (response) {
  console.log(response);
});
```