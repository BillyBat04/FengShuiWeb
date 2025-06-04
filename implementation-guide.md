# Implementation Guide - FengShuiWeb

## 1. Cấu Trúc Dự Án

### 1.1 Backend Structure
```
FengShuiWeb/
├── Application/
│   ├── DTOs/               # Data Transfer Objects
│   ├── Interfaces/         # Service interfaces
│   ├── Mappers/           # AutoMapper profiles
│   └── Services/          # Business logic implementation
├── Domain/
│   └── Models/            # Domain entities
├── Infrastructure/
│   ├── Data/              # Database context
│   ├── Repositories/      # Data access layer
│   └── DataProviders/     # External data providers
└── Presentation/
    ├── Controllers/       # API endpoints
    └── Filters/          # Action filters
```

### 1.2 Frontend Structure
```
src/
├── assets/               # Static files
├── components/
│   ├── common/          # Shared components
│   ├── analysis/        # Analysis related
│   ├── comparison/      # Comparison related
│   └── search/          # Search related
├── hooks/               # Custom React hooks
├── pages/              # Route components
├── services/           # API services
├── store/              # Redux store
├── styles/             # Global styles
└── utils/              # Helper functions
```

## 2. Setup Development Environment

### 2.1 Backend Setup
```bash
# Clone repository
git clone https://github.com/your-org/FengShuiWeb.git

# Navigate to project
cd FengShuiWeb

# Restore packages
dotnet restore

# Update database
dotnet ef database update

# Run application
dotnet run
```

### 2.2 Frontend Setup
```bash
# Navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

## 3. Implementation Steps

### 3.1 Backend Implementation

#### Database Setup
1. **Entity Framework Migrations**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

2. **Seed Data**
```csharp
public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Seed feng shui data
        if (!context.FengShuiData.Any())
        {
            context.FengShuiData.AddRange(GetInitialData());
            context.SaveChanges();
        }
    }
}
```

#### API Implementation

1. **Authentication Service**
```csharp
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtGenerator _jwtGenerator;

    public async Task<AuthResponse> SignInAsync(SignInRequest request)
    {
        // Implement sign in logic
    }
}
```

2. **Feng Shui Analysis Service**
```csharp
public class FengShuiService : IFengShuiService
{
    private readonly IFengShuiAnalysisRepository _repository;
    private readonly IGeminiService _geminiService;

    public async Task<AnalysisResponse> AnalyzeAsync(AnalysisRequest request)
    {
        // Implement analysis logic
    }
}
```

### 3.2 Frontend Implementation

#### State Management

1. **Redux Store Setup**
```typescript
// store/index.ts
import { configureStore } from '@reduxjs/toolkit';
import analysisReducer from './analysisSlice';
import comparisonReducer from './comparisonSlice';

export const store = configureStore({
  reducer: {
    analysis: analysisReducer,
    comparison: comparisonReducer
  }
});
```

2. **Analysis Slice**
```typescript
// store/analysisSlice.ts
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';

export const performAnalysis = createAsyncThunk(
  'analysis/perform',
  async (data: AnalysisRequest) => {
    const response = await api.analyze(data);
    return response.data;
  }
);

const analysisSlice = createSlice({
  name: 'analysis',
  initialState,
  reducers: {
    // Define reducers
  }
});
```

#### Component Implementation

1. **Analysis Form**
```tsx
// components/analysis/AnalysisForm.tsx
const AnalysisForm: React.FC = () => {
  const [formData, setFormData] = useState<AnalysisFormData>({
    birthYear: '',
    gender: '',
    mainDoorDirection: ''
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // Submit logic
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* Form fields */}
    </form>
  );
};
```

2. **Comparison Component**
```tsx
// components/comparison/ComparisonView.tsx
const ComparisonView: React.FC = () => {
  const analyses = useSelector(selectAnalyses);
  
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {analyses.map(analysis => (
        <ComparisonCard key={analysis.id} analysis={analysis} />
      ))}
    </div>
  );
};
```

### 3.3 Animation Implementation

1. **Page Transitions**
```tsx
// components/common/PageTransition.tsx
import { motion } from 'framer-motion';

const pageVariants = {
  initial: { opacity: 0, y: 20 },
  animate: { opacity: 1, y: 0 },
  exit: { opacity: 0, y: -20 }
};

const PageTransition: React.FC = ({ children }) => {
  return (
    <motion.div
      variants={pageVariants}
      initial="initial"
      animate="animate"
      exit="exit"
      transition={{ duration: 0.4 }}
    >
      {children}
    </motion.div>
  );
};
```

2. **Analysis Visualization**
```tsx
// components/analysis/DirectionCompass.tsx
import { Canvas } from '@react-three/fiber';

const DirectionCompass: React.FC<Props> = ({ directions }) => {
  return (
    <Canvas>
      {/* 3D compass implementation */}
    </Canvas>
  );
};
```

## 4. Testing

### 4.1 Backend Testing
```csharp
// Tests/Services/FengShuiServiceTests.cs
public class FengShuiServiceTests
{
    [Fact]
    public async Task AnalyzeAsync_ValidRequest_ReturnsAnalysis()
    {
        // Arrange
        var service = new FengShuiService(mockRepo.Object, mockGemini.Object);
        
        // Act
        var result = await service.AnalyzeAsync(request);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedKuaNumber, result.Profile.KuaNumber);
    }
}
```

### 4.2 Frontend Testing
```typescript
// tests/components/AnalysisForm.test.tsx
import { render, fireEvent, waitFor } from '@testing-library/react';

describe('AnalysisForm', () => {
  it('submits form with valid data', async () => {
    const { getByLabelText, getByRole } = render(<AnalysisForm />);
    
    fireEvent.change(getByLabelText(/birth year/i), {
      target: { value: '1990' }
    });
    
    await waitFor(() => {
      expect(mockAnalyze).toHaveBeenCalledWith(expect.any(Object));
    });
  });
});
```

## 5. Deployment

### 5.1 Backend Deployment
```bash
# Publish application
dotnet publish -c Release

# Docker build
docker build -t fengshuiweb-api .
docker run -p 7253:80 fengshuiweb-api
```

### 5.2 Frontend Deployment
```bash
# Build production bundle
npm run build

# Deploy to hosting service
npm run deploy
```

## 6. Monitoring

### 6.1 Application Monitoring
```csharp
// Startup.cs
public void Configure(IApplicationBuilder app)
{
    app.UseMetricServer();
    app.UseHttpMetrics();
    
    // Configure Prometheus metrics
}
```

### 6.2 Error Tracking
```typescript
// frontend/src/utils/errorTracking.ts
import * as Sentry from "@sentry/react";

Sentry.init({
  dsn: "your-sentry-dsn",
  environment: process.env.NODE_ENV
});
```

## 7. Performance Optimization

### 7.1 Backend Optimization
- Implement caching for feng shui calculations
- Optimize database queries
- Use async/await properly
- Implement rate limiting

### 7.2 Frontend Optimization
- Implement code splitting
- Use React.lazy for route components
- Optimize images and assets
- Implement service worker for offline support

## 8. Security Considerations

### 8.1 API Security
- Implement JWT authentication
- Use HTTPS
- Implement rate limiting
- Validate all inputs
- Use proper CORS settings

### 8.2 Frontend Security
- Sanitize user inputs
- Implement CSP
- Handle sensitive data properly
- Use secure HTTP-only cookies 