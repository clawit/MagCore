## 创建游戏

### URL
http\://106.75.33.221:6000/api/game

### Method
POST


### Header
Content-Type: application/json

### Body
{"Map":"RectSmall"}

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
});```