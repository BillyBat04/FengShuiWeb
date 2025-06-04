# API Integration Guide - FengShuiWeb Frontend

## 1. API Endpoints

### 1.1 Analysis API
```typescript
interface AnalysisEndpoints {
  // Phân tích phong thủy mới
  'POST /api/analysis': {
    request: {
      birthYear: number;
      gender: 'male' | 'female';
      direction: string;
      propertyType: 'house' | 'apartment' | 'office';
    };
    response: {
      id: string;
      kuaNumber: number;
      favorableDirections: string[];
      unfavorableDirections: string[];
      recommendations: {
        living: string[];
        working: string[];
        sleeping: string[];
      };
      createdAt: string;
    };
  };

  // Lấy chi tiết phân tích
  'GET /api/analysis/:id': {
    response: AnalysisResponse;
  };

  // Lấy lịch sử phân tích
  'GET /api/analysis/history': {
    response: {
      analyses: AnalysisResponse[];
      total: number;
    };
  };
}
```

### 1.2 Comparison API
```typescript
interface ComparisonEndpoints {
  // So sánh nhiều phân tích
  'POST /api/comparison': {
    request: {
      analysisIds: string[];
    };
    response: {
      id: string;
      analyses: AnalysisResponse[];
      compatibility: {
        score: number;
        factors: string[];
        recommendations: string[];
      };
      differences: {
        directions: DirectionDifference[];
        elements: ElementDifference[];
      };
    };
  };

  // Lấy lịch sử so sánh
  'GET /api/comparison/history': {
    response: {
      comparisons: ComparisonResponse[];
      total: number;
    };
  };
}
```

## 2. API Service Implementation

### 2.1 Base Configuration
```typescript
// api/config.ts
import axios from 'axios';

export const api = axios.create({
  baseURL: process.env.VITE_API_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Request interceptor
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Handle unauthorized
    }
    return Promise.reject(error);
  }
);
```

### 2.2 API Services
```typescript
// services/fengshui.service.ts
export const fengShuiService = {
  // Analysis
  createAnalysis: async (data: AnalysisRequest) => {
    const response = await api.post<AnalysisResponse>('/analysis', data);
    return response.data;
  },

  getAnalysis: async (id: string) => {
    const response = await api.get<AnalysisResponse>(`/analysis/${id}`);
    return response.data;
  },

  getHistory: async (params: PaginationParams) => {
    const response = await api.get('/analysis/history', { params });
    return response.data;
  },

  // Comparison
  createComparison: async (data: ComparisonRequest) => {
    const response = await api.post<ComparisonResponse>('/comparison', data);
    return response.data;
  },

  getComparisonHistory: async (params: PaginationParams) => {
    const response = await api.get('/comparison/history', { params });
    return response.data;
  }
};
```

## 3. React Query Integration

### 3.1 Query Hooks
```typescript
// hooks/useAnalysis.ts
export const useAnalysis = (id: string) => {
  return useQuery(
    ['analysis', id],
    () => fengShuiService.getAnalysis(id),
    {
      staleTime: 5 * 60 * 1000, // 5 minutes
      cacheTime: 30 * 60 * 1000 // 30 minutes
    }
  );
};

export const useAnalysisHistory = (params: PaginationParams) => {
  return useQuery(
    ['analysisHistory', params],
    () => fengShuiService.getHistory(params),
    {
      keepPreviousData: true
    }
  );
};
```

### 3.2 Mutation Hooks
```typescript
// hooks/useAnalysisMutations.ts
export const useCreateAnalysis = () => {
  const queryClient = useQueryClient();
  
  return useMutation(
    (data: AnalysisRequest) => fengShuiService.createAnalysis(data),
    {
      onSuccess: (data) => {
        queryClient.invalidateQueries('analysisHistory');
        queryClient.setQueryData(['analysis', data.id], data);
      }
    }
  );
};

export const useCreateComparison = () => {
  return useMutation(
    (data: ComparisonRequest) => fengShuiService.createComparison(data)
  );
};
```

## 4. Error Handling

### 4.1 Error Types
```typescript
// types/error.ts
export interface ApiError {
  code: string;
  message: string;
  details?: Record<string, string[]>;
}

export class FengShuiApiError extends Error {
  constructor(
    public code: string,
    message: string,
    public details?: Record<string, string[]>
  ) {
    super(message);
    this.name = 'FengShuiApiError';
  }
}
```

### 4.2 Error Handling
```typescript
// utils/error-handling.ts
export const handleApiError = (error: unknown) => {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data as ApiError;
    
    throw new FengShuiApiError(
      data.code || 'UNKNOWN_ERROR',
      data.message || 'An unknown error occurred',
      data.details
    );
  }
  
  throw error;
};
```

## 5. Data Types

### 5.1 Request Types
```typescript
// types/requests.ts
export interface AnalysisRequest {
  birthYear: number;
  gender: 'male' | 'female';
  direction: string;
  propertyType: 'house' | 'apartment' | 'office';
}

export interface ComparisonRequest {
  analysisIds: string[];
}

export interface PaginationParams {
  page: number;
  limit: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}
```

### 5.2 Response Types
```typescript
// types/responses.ts
export interface AnalysisResponse {
  id: string;
  kuaNumber: number;
  favorableDirections: string[];
  unfavorableDirections: string[];
  recommendations: {
    living: string[];
    working: string[];
    sleeping: string[];
  };
  createdAt: string;
}

export interface ComparisonResponse {
  id: string;
  analyses: AnalysisResponse[];
  compatibility: {
    score: number;
    factors: string[];
    recommendations: string[];
  };
  differences: {
    directions: DirectionDifference[];
    elements: ElementDifference[];
  };
}
```

## 6. Usage Examples

### 6.1 Using Query Hooks
```typescript
// components/AnalysisDetails.tsx
const AnalysisDetails: React.FC<{ id: string }> = ({ id }) => {
  const { data, isLoading, error } = useAnalysis(id);

  if (isLoading) return <LoadingSpinner />;
  if (error) return <ErrorMessage error={error} />;
  
  return (
    <div>
      <h2>Kết Quả Phân Tích</h2>
      <KuaNumber value={data.kuaNumber} />
      <DirectionsList directions={data.favorableDirections} />
      <RecommendationsList recommendations={data.recommendations} />
    </div>
  );
};
```

### 6.2 Using Mutation Hooks
```typescript
// components/AnalysisForm.tsx
const AnalysisForm: React.FC = () => {
  const createAnalysis = useCreateAnalysis();
  
  const handleSubmit = async (data: AnalysisRequest) => {
    try {
      const result = await createAnalysis.mutateAsync(data);
      // Handle success
    } catch (error) {
      // Handle error
    }
  };
  
  return (
    <form onSubmit={handleSubmit}>
      {/* Form fields */}
    </form>
  );
};
```

## 7. Testing

### 7.1 Mock Service
```typescript
// tests/mocks/fengshui-service.ts
export const mockFengShuiService = {
  createAnalysis: vi.fn(),
  getAnalysis: vi.fn(),
  getHistory: vi.fn(),
  createComparison: vi.fn(),
  getComparisonHistory: vi.fn()
};
```

### 7.2 Test Examples
```typescript
// tests/hooks/useAnalysis.test.ts
describe('useAnalysis', () => {
  it('fetches and returns analysis data', async () => {
    const mockData = {
      id: '123',
      kuaNumber: 4,
      // ...
    };
    
    mockFengShuiService.getAnalysis.mockResolvedValue(mockData);
    
    const { result } = renderHook(() => useAnalysis('123'));
    
    await waitFor(() => {
      expect(result.current.data).toEqual(mockData);
    });
  });
}); 