# System Architecture - FengShuiWeb Frontend

## 1. Tổng Quan Kiến Trúc

### 1.1 Tech Stack
```typescript
const techStack = {
  framework: 'React 18',
  language: 'TypeScript 5',
  stateManagement: 'Redux Toolkit',
  styling: 'Tailwind CSS',
  routing: 'React Router 6',
  buildTool: 'Vite',
  testing: 'Vitest + React Testing Library',
  animation: 'Framer Motion',
  formHandling: 'React Hook Form + Zod',
  apiClient: 'Axios + React Query'
};
```

### 1.2 Folder Structure
```
src/
├── assets/                 # Static files
├── components/            # Shared components
│   ├── analysis/         # Phong thủy analysis
│   ├── common/           # Common UI components
│   ├── layout/           # Layout components
│   └── shared/           # Shared utilities
├── config/               # App configuration
├── features/             # Feature modules
├── hooks/                # Custom hooks
├── pages/                # Route components
├── services/             # API services
├── store/                # Redux store
├── styles/               # Global styles
├── types/                # TypeScript types
└── utils/                # Utility functions
```

## 2. Core Features

### 2.1 Phong Thủy Analysis
```typescript
interface FengShuiAnalysis {
  userInput: {
    birthYear: number;
    gender: 'male' | 'female';
    direction: Direction;
  };
  calculations: {
    kuaNumber: number;
    favorableDirections: Direction[];
    unfavorableDirections: Direction[];
  };
  recommendations: {
    living: string[];
    working: string[];
    sleeping: string[];
  };
}
```

### 2.2 Comparison Feature
```typescript
interface ComparisonData {
  analyses: FengShuiAnalysis[];
  differences: {
    favorableDirections: Direction[];
    unfavorableDirections: Direction[];
    recommendations: string[];
  };
  compatibility: {
    score: number;
    factors: string[];
    suggestions: string[];
  };
}
```

## 3. State Management

### 3.1 Redux Store Structure
```typescript
interface RootState {
  analysis: {
    current: FengShuiAnalysis | null;
    history: FengShuiAnalysis[];
    loading: boolean;
    error: string | null;
  };
  comparison: {
    selectedAnalyses: string[];
    comparisonResult: ComparisonData | null;
  };
  user: {
    profile: UserProfile | null;
    preferences: UserPreferences;
  };
  ui: {
    theme: 'light' | 'dark';
    language: string;
    notifications: Notification[];
  };
}
```

### 3.2 Redux Toolkit Slices
```typescript
// Analysis Slice
const analysisSlice = createSlice({
  name: 'analysis',
  initialState,
  reducers: {
    startAnalysis: (state, action) => {},
    setResult: (state, action) => {},
    addToHistory: (state, action) => {},
    clearHistory: (state) => {}
  }
});

// Comparison Slice
const comparisonSlice = createSlice({
  name: 'comparison',
  initialState,
  reducers: {
    selectForComparison: (state, action) => {},
    setComparisonResult: (state, action) => {},
    clearComparison: (state) => {}
  }
});
```

## 4. Component Architecture

### 4.1 Smart Components
```typescript
// Container component pattern
const AnalysisContainer: React.FC = () => {
  const dispatch = useDispatch();
  const analysis = useSelector(selectAnalysis);
  
  // Business logic và state management
  
  return <AnalysisView data={analysis} onSubmit={handleSubmit} />;
};
```

### 4.2 Presentational Components
```typescript
// Pure presentation component
const AnalysisView: React.FC<AnalysisViewProps> = ({
  data,
  onSubmit
}) => {
  return (
    <div className="analysis-view">
      {/* UI elements */}
    </div>
  );
};
```

## 5. API Integration

### 5.1 API Service Structure
```typescript
// Base API configuration
const api = axios.create({
  baseURL: process.env.VITE_API_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
});

// API service
export const fengShuiService = {
  analyze: async (data: AnalysisRequest) => {
    return api.post<AnalysisResponse>('/analysis', data);
  },
  compare: async (ids: string[]) => {
    return api.post<ComparisonResponse>('/compare', { ids });
  }
};
```

### 5.2 React Query Integration
```typescript
// Custom hooks for API calls
export const useAnalysis = (id: string) => {
  return useQuery(['analysis', id], () => 
    fengShuiService.getAnalysis(id)
  );
};

export const useComparison = (ids: string[]) => {
  return useQuery(['comparison', ids], () =>
    fengShuiService.compare(ids)
  );
};
```

## 6. Routing Structure

### 6.1 Route Configuration
```typescript
const routes = [
  {
    path: '/',
    element: <Layout />,
    children: [
      {
        path: 'analysis',
        element: <AnalysisPage />,
        children: [
          {
            path: 'new',
            element: <NewAnalysis />
          },
          {
            path: ':id',
            element: <AnalysisDetails />
          }
        ]
      },
      {
        path: 'comparison',
        element: <ComparisonPage />,
        children: [
          {
            path: 'new',
            element: <NewComparison />
          }
        ]
      }
    ]
  }
];
```

### 6.2 Navigation Guards
```typescript
const ProtectedRoute: React.FC = ({ children }) => {
  const user = useSelector(selectUser);
  const location = useLocation();

  if (!user) {
    return <Navigate to="/login" state={{ from: location }} />;
  }

  return children;
};
```

## 7. Performance Optimization

### 7.1 Code Splitting
```typescript
// Lazy loading routes
const AnalysisPage = lazy(() => import('./pages/AnalysisPage'));
const ComparisonPage = lazy(() => import('./pages/ComparisonPage'));

// Suspense wrapper
const App = () => (
  <Suspense fallback={<LoadingSpinner />}>
    <Routes>{/* ... */}</Routes>
  </Suspense>
);
```

### 7.2 Memoization
```typescript
// Memoized selectors
export const selectFilteredAnalyses = createSelector(
  [selectAnalyses, selectFilters],
  (analyses, filters) => {
    // Complex filtering logic
  }
);

// Memoized components
export const AnalysisCard = memo<AnalysisCardProps>(({
  analysis,
  onSelect
}) => {
  // Render logic
});
```

## 8. Testing Strategy

### 8.1 Component Testing
```typescript
// Component test example
describe('AnalysisForm', () => {
  it('validates input correctly', async () => {
    render(<AnalysisForm />);
    
    // Test implementation
  });
  
  it('submits form data', async () => {
    // Test implementation
  });
});
```

### 8.2 Integration Testing
```typescript
// Integration test example
describe('Analysis Flow', () => {
  it('completes full analysis process', async () => {
    // Test implementation
  });
});
```

## 9. Error Handling

### 9.1 Global Error Boundary
```typescript
class ErrorBoundary extends React.Component {
  static getDerivedStateFromError(error: Error) {
    return { hasError: true, error };
  }

  render() {
    if (this.state.hasError) {
      return <ErrorView error={this.state.error} />;
    }
    return this.props.children;
  }
}
```

### 9.2 API Error Handling
```typescript
const handleApiError = (error: unknown) => {
  if (axios.isAxiosError(error)) {
    // Handle API errors
  } else {
    // Handle other errors
  }
};
``` 