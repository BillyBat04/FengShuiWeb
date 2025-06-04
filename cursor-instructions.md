# Cursor IDE Instructions - FengShuiWeb

## 1. Cài Đặt và Cấu Hình

### 1.1 Cài Đặt Cursor
1. Tải Cursor từ [https://cursor.sh](https://cursor.sh)
2. Cài đặt và khởi động Cursor
3. Đăng nhập với tài khoản GitHub (nếu cần)

### 1.2 Cấu Hình Dự Án
1. Mở dự án FengShuiWeb trong Cursor
2. Cấu hình các extensions cần thiết:
   - C# Extension
   - TypeScript and JavaScript Language Features
   - ESLint
   - Prettier
   - Tailwind CSS IntelliSense

### 1.3 Workspace Settings
```json
{
  "editor.formatOnSave": true,
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": true
  },
  "typescript.updateImportsOnFileMove.enabled": "always",
  "typescript.suggest.autoImports": true,
  "csharp.format.enable": true
}
```

## 2. Tính Năng Chính

### 2.1 AI Code Completion
- Sử dụng `Ctrl + Space` để kích hoạt gợi ý code
- Sử dụng `Tab` để chấp nhận gợi ý
- Sử dụng `Alt + [` và `Alt + ]` để di chuyển giữa các gợi ý

### 2.2 AI Chat
- Mở chat panel với `Ctrl + Shift + L`
- Hỏi về code với cú pháp:
  ```
  /explain [selected code]
  /refactor [selected code]
  /test [selected code]
  ```

### 2.3 Code Navigation
- `Ctrl + Click`: Đi đến định nghĩa
- `Alt + Left`: Quay lại vị trí trước
- `Ctrl + Shift + O`: Tìm kiếm symbols
- `Ctrl + P`: Tìm kiếm files

## 3. Quy Trình Làm Việc

### 3.1 Backend Development
1. **Tạo Model mới**
   ```csharp
   // Gõ "model" và để Cursor gợi ý template
   public class NewModel
   {
       // Gõ "prop" cho property template
   }
   ```

2. **Tạo Controller**
   ```csharp
   // Gõ "controller" cho template
   [ApiController]
   [Route("api/[controller]")]
   public class NewController : ControllerBase
   {
       // Gõ "action" cho action method template
   }
   ```

3. **Tạo Service**
   ```csharp
   // Gõ "service" cho template
   public class NewService : INewService
   {
       // Gõ "ctor" cho constructor
   }
   ```

### 3.2 Frontend Development
1. **Tạo Component**
   ```tsx
   // Gõ "rfc" cho React Function Component template
   const NewComponent: React.FC = () => {
     return (
       <div>
         {/* Cursor sẽ gợi ý JSX elements */}
       </div>
     );
   };
   ```

2. **Tạo Hook**
   ```tsx
   // Gõ "hook" cho custom hook template
   const useCustomHook = () => {
     // Cursor sẽ gợi ý React hooks
   };
   ```

3. **Tạo Slice**
   ```tsx
   // Gõ "slice" cho Redux Toolkit slice template
   const newSlice = createSlice({
     name: 'new',
     initialState,
     reducers: {
       // Cursor sẽ gợi ý reducer template
     }
   });
   ```

## 4. Code Snippets

### 4.1 Backend Snippets
```json
{
  "Create DTO": {
    "prefix": "dto",
    "body": [
      "public class ${1:Name}Dto",
      "{",
      "    $0",
      "}"
    ]
  },
  "Create Service Method": {
    "prefix": "svcmethod",
    "body": [
      "public async Task<${1:ReturnType}> ${2:MethodName}Async(${3:Type} ${4:param})",
      "{",
      "    $0",
      "}"
    ]
  }
}
```

### 4.2 Frontend Snippets
```json
{
  "Create Component": {
    "prefix": "comp",
    "body": [
      "import React from 'react';",
      "",
      "interface ${1:Name}Props {",
      "  $2",
      "}",
      "",
      "export const ${1:Name}: React.FC<${1:Name}Props> = ({ $3 }) => {",
      "  return (",
      "    <div>",
      "      $0",
      "    </div>",
      "  );",
      "};"
    ]
  }
}
```

## 5. Debug Configuration

### 5.1 Backend Debug
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (web)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/bin/Debug/net8.0/FengShuiWeb.dll",
      "args": [],
      "cwd": "${workspaceFolder}",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      }
    }
  ]
}
```

### 5.2 Frontend Debug
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "type": "chrome",
      "request": "launch",
      "name": "Launch Chrome against localhost",
      "url": "http://localhost:3000",
      "webRoot": "${workspaceFolder}/frontend"
    }
  ]
}
```

## 6. Best Practices

### 6.1 Code Organization
- Sử dụng region folding cho code sections
- Đặt tên files theo chuẩn PascalCase cho components
- Tổ chức imports theo nhóm (React, Third-party, Local)

### 6.2 Commenting
- Sử dụng XML comments cho C# code
- Sử dụng JSDoc cho TypeScript/JavaScript
- Thêm TODO comments cho công việc cần hoàn thiện

### 6.3 Git Integration
- Sử dụng Source Control panel (`Ctrl + Shift + G`)
- Commit message format:
  ```
  type(scope): description
  
  [optional body]
  ```
- Tạo branch mới: `Ctrl + Shift + P` > "Git: Create Branch"

## 7. Troubleshooting

### 7.1 Common Issues
1. **IntelliSense không hoạt động**
   - Reload window: `Ctrl + Shift + P` > "Reload Window"
   - Xóa cache: Delete `.vs` folder

2. **Build errors**
   - Clean solution
   - Restore NuGet packages
   - Delete `bin` và `obj` folders

3. **Frontend dev server issues**
   - Clear npm cache
   - Delete `node_modules`
   - Run `npm install`

### 7.2 Performance Tips
- Disable unused extensions
- Use workspace trust
- Regular garbage collection
- Keep Git history clean 

# Hướng Dẫn Sử Dụng Cursor cho Frontend FengShuiWeb

## 1. Cấu Trúc Frontend

```
frontend/
├── src/
│   ├── components/
│   │   ├── analysis/          # Components phân tích phong thủy
│   │   ├── comparison/        # Components so sánh kết quả
│   │   ├── layout/           # Components layout chung
│   │   └── shared/           # Components dùng chung
│   ├── hooks/                # Custom React hooks
│   ├── pages/                # Các trang chính
│   ├── services/             # API services
│   ├── store/                # Redux store
│   ├── styles/               # Global styles
│   └── utils/                # Helper functions
└── package.json
```

## 2. Sử Dụng AI Trong Cursor

### 2.1 Tạo Component Mới
1. Gõ `/create` và chọn "Create React Component"
2. Nhập tên component, ví dụ: `FengShuiAnalysis`
3. Cursor sẽ tạo component với TypeScript và Tailwind CSS:
```tsx
import React from 'react';

interface FengShuiAnalysisProps {
  birthYear: number;
  direction: string;
  // Cursor sẽ gợi ý các props phù hợp
}

export const FengShuiAnalysis: React.FC<FengShuiAnalysisProps> = ({
  birthYear,
  direction,
}) => {
  return (
    <div className="p-4 bg-white rounded-lg shadow-md">
      {/* Cursor sẽ gợi ý JSX và Tailwind classes */}
    </div>
  );
};
```

### 2.2 Tạo Custom Hook
1. Gõ `/create` và chọn "Create React Hook"
2. Nhập tên hook, ví dụ: `useFengShuiCalculation`
3. Cursor sẽ tạo hook với TypeScript:
```tsx
import { useState, useEffect } from 'react';
import { calculateKuaNumber } from '../utils/fengshui';

export const useFengShuiCalculation = (birthYear: number, gender: string) => {
  // Cursor sẽ gợi ý logic phong thủy
};
```

### 2.3 Tạo API Service
1. Gõ `/create` và chọn "Create API Service"
2. Nhập tên service: `fengShuiService`
3. Cursor sẽ tạo service với Axios:
```tsx
import axios from 'axios';
import { AnalysisRequest, AnalysisResponse } from '../types';

const API_URL = process.env.REACT_APP_API_URL;

export const fengShuiService = {
  analyze: async (data: AnalysisRequest): Promise<AnalysisResponse> => {
    // Cursor sẽ gợi ý implementation
  }
};
```

## 3. Phím Tắt Hữu Ích

### 3.1 Code Navigation
- `Ctrl + P`: Tìm file
- `Ctrl + Shift + F`: Tìm trong toàn bộ project
- `Ctrl + Click`: Đi đến định nghĩa
- `Alt + ←`: Quay lại vị trí trước

### 3.2 Code Generation
- `Ctrl + Space`: Kích hoạt AI suggestions
- `Ctrl + Enter`: Chấp nhận suggestion
- `/`: Mở menu lệnh AI

### 3.3 Refactoring
- `Ctrl + .`: Quick fixes và refactoring
- `F2`: Rename symbol
- `Alt + Shift + R`: Extract component

## 4. AI Commands Cho Frontend

### 4.1 Component Development
```
/explain component structure
/create new component
/add props to component
/implement component logic
/style component with Tailwind
```

### 4.2 State Management
```
/create redux slice
/implement reducer
/add async thunk
/connect component to store
```

### 4.3 Testing
```
/create test file
/add test case
/generate mock data
/test async function
```

## 5. Best Practices với Cursor

### 5.1 Component Structure
```tsx
// Cursor sẽ giúp tạo cấu trúc này
import React from 'react';
import { useDispatch } from 'react-redux';
import { useFengShuiCalculation } from '../hooks';
import { AnalysisResult } from './AnalysisResult';

interface Props {
  // Props interface
}

export const Component: React.FC<Props> = () => {
  // State và hooks
  
  // Event handlers
  
  // Side effects
  
  // Render helpers
  
  // Main render
};
```

### 5.2 Custom Hooks
```tsx
// Cursor sẽ giúp tổ chức logic
export const useCustomHook = (params) => {
  // State
  
  // Memoized values
  
  // Side effects
  
  // Event handlers
  
  // Return values
};
```

### 5.3 API Integration
```tsx
// Cursor sẽ giúp implement
export const apiCall = async () => {
  try {
    // Request configuration
    
    // API call
    
    // Response handling
    
    // Error handling
  } catch (error) {
    // Error processing
  }
};
```

## 6. Debugging với Cursor

### 6.1 Console Logging
```tsx
// Cursor sẽ gợi ý logging points
console.log('Component rendered:', { props, state });
console.log('API response:', response);
console.error('Error occurred:', error);
```

### 6.2 React DevTools Integration
- Sử dụng Components tab
- Monitoring state changes
- Profiling renders

### 6.3 Network Debugging
- Monitoring API calls
- Checking request/response
- Error tracking

## 7. Tips & Tricks

### 7.1 Code Generation
- Sử dụng AI để tạo boilerplate code
- Tận dụng snippets có sẵn
- Để AI đề xuất tối ưu hóa

### 7.2 Code Quality
- Sử dụng ESLint integration
- Format code tự động với Prettier
- Kiểm tra types với TypeScript

### 7.3 Performance
- Lazy loading components
- Memoization với useMemo và useCallback
- Code splitting

## 8. Troubleshooting

### 8.1 Common Issues
- TypeScript errors
- Component rendering issues
- State management problems
- API integration errors

### 8.2 Solutions
- Sử dụng AI để debug
- Check console logs
- Verify network calls
- Review component lifecycle 