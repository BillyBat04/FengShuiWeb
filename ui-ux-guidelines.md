# UI/UX Guidelines - FengShuiWeb Frontend

## 1. Design System

### 1.1 Brand Identity
```scss
// Brand Colors
$brand-colors: {
  primary: {
    base: #FF6B6B,     // Màu chủ đạo - Tượng trưng cho may mắn
    light: #FFE3E3,
    dark: #CC5555
  },
  secondary: {
    base: #4ECDC4,    // Màu phụ - Tượng trưng cho sự cân bằng
    light: #A8E6E2,
    dark: #3EA49D
  },
  accent: {
    gold: #FFD700,    // Tượng trưng cho sự thịnh vượng
    red: #FF4D4D,     // Tượng trưng cho may mắn
    green: #4CAF50    // Tượng trưng cho sự tăng trưởng
  }
};

// Typography
$typography: {
  primary: 'Noto Serif', // Font chữ chính - Mang phong cách Á Đông
  secondary: 'Playfair Display', // Font phụ - Tăng tính sang trọng
  system: 'Inter' // Font hệ thống - Dễ đọc
};

// Shadows & Effects
$effects: {
  elevation: {
    low: '0 2px 4px rgba(0,0,0,0.1)',
    medium: '0 4px 8px rgba(0,0,0,0.12)',
    high: '0 8px 16px rgba(0,0,0,0.14)'
  },
  gradient: {
    primary: 'linear-gradient(135deg, #FF6B6B 0%, #FFE3E3 100%)',
    secondary: 'linear-gradient(135deg, #4ECDC4 0%, #A8E6E2 100%)'
  }
};
```

### 1.2 Design Tokens
```typescript
export const tokens = {
  spacing: {
    xs: '0.25rem',    // 4px
    sm: '0.5rem',     // 8px
    md: '1rem',       // 16px
    lg: '1.5rem',     // 24px
    xl: '2rem',       // 32px
    xxl: '2.5rem'     // 40px
  },
  borderRadius: {
    sm: '0.25rem',    // 4px
    md: '0.5rem',     // 8px
    lg: '1rem',       // 16px
    full: '9999px'
  },
  animation: {
    duration: {
      fast: '150ms',
      normal: '300ms',
      slow: '500ms'
    },
    easing: {
      easeOut: 'cubic-bezier(0.4, 0, 0.2, 1)',
      easeInOut: 'cubic-bezier(0.4, 0, 0.2, 1)',
      spring: 'cubic-bezier(0.175, 0.885, 0.32, 1.275)'
    }
  }
};
```

## 2. Layout & Grid System

### 2.1 Grid Configuration
```scss
.grid-system {
  // Desktop Grid
  @media (min-width: 1024px) {
    display: grid;
    grid-template-columns: repeat(12, 1fr);
    gap: 2rem;
    padding: 0 4rem;
  }

  // Tablet Grid
  @media (min-width: 768px) and (max-width: 1023px) {
    display: grid;
    grid-template-columns: repeat(8, 1fr);
    gap: 1.5rem;
    padding: 0 2rem;
  }

  // Mobile Grid
  @media (max-width: 767px) {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 1rem;
    padding: 0 1rem;
  }
}
```

### 2.2 Layout Components
```typescript
// Layout components with animations
export const PageLayout = styled.div`
  opacity: 0;
  animation: ${fadeIn} 0.5s ease-out forwards;
  
  @media (min-width: 1024px) {
    padding: ${tokens.spacing.xl};
  }
`;

export const Section = styled.section`
  margin: ${tokens.spacing.xl} 0;
  transform: translateY(20px);
  opacity: 0;
  animation: ${slideUp} 0.5s ease-out forwards;
`;
```

## 3. Component Design Patterns

### 3.1 Navigation & Header
```typescript
// Modern Navigation with Animations
export const Navigation = styled.nav`
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(10px);
  position: sticky;
  top: 0;
  z-index: 100;
  
  // Hover effect for menu items
  .nav-item {
    position: relative;
    
    &::after {
      content: '';
      position: absolute;
      bottom: -2px;
      left: 0;
      width: 0;
      height: 2px;
      background: ${colors.primary.base};
      transition: width 0.3s ${tokens.animation.easing.easeOut};
    }
    
    &:hover::after {
      width: 100%;
    }
  }
`;
```

### 3.2 Form Elements
```typescript
// Custom Form Components
export const Input = styled.input`
  border: 2px solid transparent;
  border-radius: ${tokens.borderRadius.md};
  padding: ${tokens.spacing.md};
  transition: all 0.3s ease;
  
  &:focus {
    border-color: ${colors.primary.base};
    box-shadow: 0 0 0 3px ${colors.primary.light};
  }
`;

export const Button = styled.button`
  background: ${colors.primary.base};
  color: white;
  padding: ${tokens.spacing.md} ${tokens.spacing.lg};
  border-radius: ${tokens.borderRadius.md};
  transform: translateY(0);
  transition: all 0.3s ${tokens.animation.easing.spring};
  
  &:hover {
    transform: translateY(-2px);
    box-shadow: ${effects.elevation.medium};
  }
`;
```

## 4. Animation & Interaction Design

### 4.1 Page Transitions
```typescript
// Framer Motion Variants
export const pageVariants = {
  initial: {
    opacity: 0,
    y: 20
  },
  animate: {
    opacity: 1,
    y: 0,
    transition: {
      duration: 0.6,
      ease: [0.6, -0.05, 0.01, 0.99]
    }
  },
  exit: {
    opacity: 0,
    y: -20,
    transition: {
      duration: 0.4
    }
  }
};

// Component transitions
export const componentVariants = {
  hidden: { opacity: 0, y: 20 },
  visible: {
    opacity: 1,
    y: 0,
    transition: {
      duration: 0.6,
      ease: "easeOut"
    }
  }
};
```

### 4.2 Micro-interactions
```typescript
// Interactive Elements
export const InteractiveCard = styled(motion.div)`
  cursor: pointer;
  padding: ${tokens.spacing.lg};
  border-radius: ${tokens.borderRadius.lg};
  background: white;
  box-shadow: ${effects.elevation.low};
  
  &:hover {
    box-shadow: ${effects.elevation.medium};
    transform: translateY(-4px);
  }
  
  // Hover state animation
  transition: all 0.3s ${tokens.animation.easing.spring};
`;

// Loading States
export const LoadingSpinner = styled(motion.div)`
  @keyframes spin {
    to { transform: rotate(360deg); }
  }
  
  animation: spin 1s linear infinite;
`;
```

## 5. Advanced UI Features

### 5.1 3D Elements
```typescript
// Three.js Integration for Direction Compass
export const DirectionCompass = () => {
  return (
    <Canvas>
      <ambientLight intensity={0.5} />
      <pointLight position={[10, 10, 10]} />
      <Suspense fallback={<LoadingSpinner />}>
        <CompassModel 
          rotation={[0, 0, 0]}
          scale={[1, 1, 1]}
        />
      </Suspense>
      <OrbitControls enableZoom={false} />
    </Canvas>
  );
};
```

### 5.2 Custom Visualizations
```typescript
// D3.js Integration for Charts
export const ElementChart = () => {
  return (
    <svg width={400} height={400}>
      <g transform="translate(200, 200)">
        {/* Custom D3 visualization */}
      </g>
    </svg>
  );
};
```

## 6. Responsive Design Strategy

### 6.1 Mobile-First Approach
```scss
// Base styles (mobile)
.component {
  width: 100%;
  padding: ${tokens.spacing.md};
  
  // Tablet styles
  @media (min-width: 768px) {
    width: 50%;
    padding: ${tokens.spacing.lg};
  }
  
  // Desktop styles
  @media (min-width: 1024px) {
    width: 33.33%;
    padding: ${tokens.spacing.xl};
  }
}
```

### 6.2 Touch Interactions
```typescript
// Touch-friendly components
export const TouchableCard = styled.div`
  // Larger touch targets
  padding: ${tokens.spacing.lg};
  min-height: 44px;
  
  // Remove hover effects on touch devices
  @media (hover: none) {
    &:hover {
      transform: none;
    }
  }
`;
```

## 7. Performance Optimization

### 7.1 Loading Strategies
```typescript
// Lazy loading components
const LazyComponent = lazy(() => import('./Component'));

// Skeleton loading
export const Skeleton = styled.div`
  background: linear-gradient(
    90deg,
    ${colors.neutral.light} 0%,
    ${colors.neutral.lighter} 50%,
    ${colors.neutral.light} 100%
  );
  background-size: 200% 100%;
  animation: shimmer 1.5s infinite;
`;
```

### 7.2 Animation Performance
```typescript
// Use transform instead of position/dimensions
export const OptimizedAnimation = styled.div`
  transform: translate3d(0, 0, 0);
  will-change: transform;
  
  &:hover {
    transform: translate3d(0, -10px, 0);
  }
`;
```

## 8. Accessibility Enhancements

### 8.1 ARIA Integration
```typescript
// Accessible components
export const AccessibleButton = styled.button.attrs({
  role: 'button',
  'aria-pressed': props => props.isActive
})`
  // Styles
`;

export const AccessibleMenu = styled.nav.attrs({
  role: 'navigation',
  'aria-label': 'Main menu'
})`
  // Styles
`;
```

### 8.2 Keyboard Navigation
```typescript
// Keyboard focus styles
export const FocusableElement = styled.div`
  &:focus-visible {
    outline: 2px solid ${colors.primary.base};
    outline-offset: 2px;
  }
  
  // Remove focus ring for mouse users
  &:focus:not(:focus-visible) {
    outline: none;
  }
`;
```

## 9. Implementation Guide

### 9.1 Component Usage
```typescript
// Example implementation
const AnalysisPage = () => {
  return (
    <PageLayout>
      <motion.div variants={pageVariants}>
        <Section>
          <h1>Phân Tích Phong Thủy</h1>
          <InteractiveCard>
            <DirectionCompass />
          </InteractiveCard>
        </Section>
      </motion.div>
    </PageLayout>
  );
};
```

### 9.2 Theme Integration
```typescript
// Theme provider setup
export const theme = {
  colors,
  typography,
  effects,
  tokens,
  // Add more theme values
};

// Usage in components
const StyledComponent = styled.div`
  color: ${({ theme }) => theme.colors.primary.base};
  font-family: ${({ theme }) => theme.typography.primary};
  // More styles
`;
``` 