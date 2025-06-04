# API Documentation - FengShuiWeb

## Base URL
```
Development: https://localhost:7200
Production: https://api.fengshuiweb.com/v1
```

## Response Format
Tất cả các API endpoints đều trả về response theo format:
```json
{
  "success": boolean,
  "data": any,
  "error": {
    "code": string,
    "message": string,
    "details": any
  }
}
```

## Authentication
Tất cả các API endpoints đều yêu cầu xác thực JWT. Token cần được gửi trong header:
```
Authorization: Bearer {token}
```

### Social Authentication
```http
GET /auth/google
GET /auth/facebook
```
Redirect tới trang đăng nhập của provider tương ứng và trả về JWT token sau khi xác thực thành công.

## WebSocket Events
```
ws://localhost:7200/ws
```

### Event Format
```json
{
  "type": "string",
  "data": any
}
```

### Supported Events
- `analysis_complete`: Khi hoàn thành phân tích phong thủy
- `article_update`: Khi có bài viết mới hoặc cập nhật
- `notification`: Thông báo hệ thống

## Endpoints

### 1. Phân Tích Phong Thủy

#### 1.1. Tạo Phân Tích Mới
```http
POST /analysis
```

**Request Body:**
```json
{
  "birthYear": 1990,
  "gender": "Male",
  "mainDoorDirection": "North"
}
```

**Response:**
```json
{
  "id": 1,
  "profile": {
    "birthYear": "1990 (Canh Ngọ - Ngựa)",
    "destinyElement": "Kim",
    "kuaNumber": 7,
    "kuaGroup": "Tây Tứ Mệnh"
  },
  "directions": [
    {
      "direction": "North",
      "meaning": "Sinh Khí",
      "isAuspicious": true
    }
  ],
  "layoutTips": [
    {
      "section": "Cửa Chính",
      "tip": "Hướng cửa chính tốt cho sự thịnh vượng"
    }
  ],
  "friendlyExplanation": [
    "Giải thích chi tiết về profile",
    "Giải thích về các hướng",
    "Giải thích về cách bố trí"
  ],
  "recommendations": {
    "articles": [
      "Phong Thủy Phòng Ngủ: /articles/phong-ngu"
    ],
    "searchKeywords": "phong thủy phòng ngủ tây tứ mệnh"
  }
}
```

#### 1.2. Lưu Phân Tích
```http
POST /analysis/save
```

**Request Body:**
```json
{
  "label": "Nhà Mới Q7",
  "analysisData": "{...}" // Kết quả phân tích dạng JSON string
}
```

**Response:**
```json
{
  "id": 1,
  "message": "Đã lưu phân tích thành công"
}
```

### 2. Quản Lý Phân Tích

#### 2.1. Lấy Danh Sách Phân Tích
```http
GET /analysis/list
```

**Response:**
```json
{
  "analyses": [
    {
      "id": 1,
      "label": "Nhà Mới Q7",
      "createdAt": "2024-03-20T10:00:00Z",
      "analysisData": "{...}"
    }
  ]
}
```

#### 2.2. Tìm Kiếm Phân Tích
```http
POST /analysis/search
```

**Request Body:**
```json
{
  "label": "nhà mới",
  "dateFrom": "2024-01-01T00:00:00Z",
  "dateTo": "2024-03-21T00:00:00Z",
  "keywordInAnalysis": "phòng ngủ"
}
```

**Response:**
```json
{
  "analyses": [
    {
      "id": 1,
      "label": "Nhà Mới Q7",
      "createdAt": "2024-03-20T10:00:00Z",
      "analysisData": "{...}"
    }
  ]
}
```

#### 2.3. So Sánh Phân Tích
```http
POST /analysis/compare
```

**Request Body:**
```json
{
  "analysisIds": [1, 2]
}
```

**Response:**
```json
{
  "analyses": [
    {
      "id": 1,
      "label": "Nhà Mới Q7",
      "analysisData": "{...}"
    },
    {
      "id": 2,
      "label": "Nhà Cũ Q1",
      "analysisData": "{...}"
    }
  ],
  "advice": {
    "comparison": "Phân tích chi tiết so sánh hai địa điểm...",
    "advantages": [
      "Ưu điểm địa điểm 1...",
      "Ưu điểm địa điểm 2..."
    ],
    "disadvantages": [
      "Nhược điểm địa điểm 1...",
      "Nhược điểm địa điểm 2..."
    ],
    "recommendations": [
      "Đề xuất cải thiện địa điểm 1...",
      "Đề xuất cải thiện địa điểm 2..."
    ],
    "conclusion": "Kết luận về lựa chọn tốt nhất..."
  }
}
```

### 3. Quản Lý Người Dùng

#### 3.1. Đăng Ký
```http
POST /auth/register
```

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123",
  "fullName": "Nguyễn Văn A"
}
```

#### 3.2. Đăng Nhập
```http
POST /auth/login
```

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 3600
}
```

## Mã Lỗi

| Mã | Mô Tả |
|----|--------|
| 200 | Thành công |
| 400 | Yêu cầu không hợp lệ |
| 401 | Chưa xác thực |
| 403 | Không có quyền truy cập |
| 404 | Không tìm thấy |
| 500 | Lỗi server |

## Security

### CORS
Allowed origins:
- http://localhost:3000
- https://fengshuiweb.com

### API Key
- Required for certain endpoints
- Header: X-API-Key
- Contact admin for API key

### Rate Limiting
- Implementation: Token bucket algorithm
- Headers:
  - X-RateLimit-Limit
  - X-RateLimit-Remaining
  - X-RateLimit-Reset
- Limits:
  - 100 requests/phút cho mỗi IP
  - 1000 requests/ngày cho mỗi user

## Versioning
- API version được chỉ định trong URL path
- Version header: X-API-Version
- Phiên bản hiện tại: `v1` 