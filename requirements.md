# Yêu Cầu Hệ Thống FengShuiWeb

## 1. Tổng Quan
FengShuiWeb là một ứng dụng web hiện đại giúp người dùng phân tích và tối ưu hóa phong thủy cho không gian sống và làm việc. Ứng dụng kết hợp kiến thức phong thủy truyền thống với công nghệ AI để đưa ra những phân tích và đề xuất phù hợp nhất cho người dùng.

## 2. Chức Năng Chính

### 2.1. Phân Tích Phong Thủy
- Phân tích phong thủy dựa trên:
  - Năm sinh
  - Giới tính
  - Hướng cửa chính
  - Các yếu tố khác của không gian
- Tính toán số Kua và xác định nhóm Đông/Tây tứ mệnh
- Phân tích các hướng tốt/xấu
- Đề xuất cách bố trí nội thất và cải thiện
- Giải thích chi tiết bằng ngôn ngữ dễ hiểu
- Tích hợp AI (Gemini) để cung cấp phân tích sâu sắc

### 2.2. Quản Lý Phân Tích
- Lưu trữ các phân tích phong thủy
- Đặt tên/nhãn cho từng phân tích
- Xem lại lịch sử phân tích
- Tìm kiếm phân tích theo:
  - Tên/nhãn
  - Khoảng thời gian
  - Từ khóa trong nội dung
- So sánh nhiều phân tích:
  - So sánh ưu/nhược điểm
  - Đánh giá mức độ phù hợp
  - Đề xuất cải thiện
  - Kết luận lựa chọn tốt nhất

### 2.3. Tư Vấn và Đề Xuất
- Đề xuất bài viết liên quan
- Gợi ý từ khóa tìm kiếm
- Tư vấn cải thiện phong thủy
- So sánh và đánh giá nhiều không gian

### 2.4. Giao Diện Người Dùng
- **Hiệu Ứng và Animation**
  - Transitions mượt mà giữa các trang
  - Loading states với skeleton screens
  - Micro-interactions cho feedback
  - 3D effects cho visualization
  - Smooth scrolling experience
  
- **Trải Nghiệm Người Dùng**
  - Step-by-step wizards cho phân tích
  - Real-time validation và feedback
  - Infinite scroll cho danh sách
  - Drag-and-drop functionality
  - Touch-friendly interactions

- **Responsive Design**
  - Mobile-first approach
  - Adaptive layouts
  - Touch-optimized controls
  - Offline capabilities
  - Progressive enhancement

## 3. Yêu Cầu Phi Chức Năng

### 3.1. Hiệu Năng
- Thời gian phản hồi API < 2 giây
- Xử lý đồng thời nhiều yêu cầu
- Tối ưu hóa truy vấn cơ sở dữ liệu

### 3.2. Bảo Mật
- Xác thực người dùng
- Bảo vệ thông tin cá nhân
- Mã hóa dữ liệu nhạy cảm
- Kiểm soát quyền truy cập

### 3.3. Giao Diện
- Thiết kế responsive
- Giao diện thân thiện, dễ sử dụng
- Hỗ trợ đa ngôn ngữ (Tiếng Việt, Tiếng Anh)
- Hiển thị trực quan các phân tích

### 3.4. Khả Năng Mở Rộng
- Kiến trúc module hóa
- Dễ dàng thêm tính năng mới
- Hỗ trợ tích hợp với các hệ thống khác
- Khả năng xử lý tăng trưởng dữ liệu

## 4. Công Nghệ Sử Dụng

### 4.1. Backend
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server/SQLite
- AutoMapper
- Gemini API

### 4.2. Frontend
- React + TypeScript
- Material-UI
- Redux Toolkit
- React Query
- Framer Motion
- GSAP
- Three.js/React Three Fiber
- Locomotive Scroll
- Tailwind CSS

### 4.3. DevOps
- Git
- GitHub Actions
- Docker
- Azure/AWS

## 5. Kế Hoạch Triển Khai

### 5.1. Giai Đoạn 1
- Xây dựng core backend API
- Thiết kế cơ sở dữ liệu
- Tích hợp Gemini API
- Phát triển giao diện cơ bản

### 5.2. Giai Đoạn 2
- Phát triển tính năng so sánh
- Cải thiện UX/UI với animations
- Tối ưu hóa hiệu năng
- Thêm tính năng tìm kiếm nâng cao
- Tích hợp 3D visualization

### 5.3. Giai Đoạn 3
- Triển khai hệ thống đề xuất
- Tích hợp thêm các API bên ngoài
- Mở rộng tính năng phân tích
- Tối ưu hóa SEO 

### 5.4. Monitoring và Analytics
- Real User Monitoring (RUM)
- Error tracking và logging
- Performance metrics
- User behavior analytics
- A/B testing framework 