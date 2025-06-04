# Design Patterns & Best Practices - FengShuiWeb Frontend

## 1. Visual Hierarchy

### 1.1 Typography Scale
```scss
// Modular scale for typography
$type-scale: (
  h1: 2.5rem,    // 40px
  h2: 2rem,      // 32px
  h3: 1.75rem,   // 28px
  h4: 1.5rem,    // 24px
  body: 1rem,    // 16px
  small: 0.875rem // 14px
);

// Line heights
$line-heights: (
  tight: 1.2,
  normal: 1.5,
  loose: 1.8
);

// Font weights
$font-weights: (
  light: 300,
  regular: 400,
  medium: 500,
  bold: 700
);
```

### 1.2 Visual Components
```typescript
// Card components with visual hierarchy
export const Card = styled.div<CardProps>`
  padding: ${({ size = 'medium' }) => tokens.spacing[size]};
  border-radius: ${tokens.borderRadius.lg};
  background: ${({ variant = 'primary' }) => colors[variant].background};
  
  h3 {
    font-size: ${typeScale.h3};
    margin-bottom: ${tokens.spacing.md};
    color: ${colors.text.primary};
  }
  
  p {
    font-size: ${typeScale.body};
    color: ${colors.text.secondary};
    line-height: ${lineHeights.normal};
  }
`;
```

## 2. Motion Design

### 2.1 Animation Principles
```typescript
// Animation timing functions
export const easings = {
  easeOutCubic: 'cubic-bezier(0.33, 1, 0.68, 1)',
  easeOutBack: 'cubic-bezier(0.34, 1.56, 0.64, 1)',
  springy: 'cubic-bezier(0.68, -0.6, 0.32, 1.6)'
};

// Reusable animations
export const animations = {
  fadeIn: keyframes`
    from { opacity: 0; }
    to { opacity: 1; }
  `,
  slideUp: keyframes`
    from { 
      transform: translateY(20px);
      opacity: 0;
    }
    to { 
      transform: translateY(0);
      opacity: 1;
    }
  `,
  scaleIn: keyframes`
    from { 
      transform: scale(0.95);
      opacity: 0;
    }
    to { 
      transform: scale(1);
      opacity: 1;
    }
  `
};
```

### 2.2 Interaction States
```typescript
// Interactive element states
export const InteractiveElement = styled.button`
  // Default state
  position: relative;
  transition: all 0.3s ${easings.easeOutCubic};
  
  // Hover state
  &:hover {
    transform: translateY(-2px);
    box-shadow: ${shadows.medium};
  }
  
  // Active state
  &:active {
    transform: translateY(0);
    box-shadow: ${shadows.small};
  }
  
  // Focus state
  &:focus-visible {
    outline: 2px solid ${colors.primary.base};
    outline-offset: 2px;
  }
`;
```

## 3. Layout Patterns

### 3.1 Grid System
```typescript
// Flexible grid system
export const Grid = styled.div<GridProps>`
  display: grid;
  grid-template-columns: repeat(
    ${({ columns = 12 }) => columns},
    1fr
  );
  gap: ${({ gap = 'md' }) => tokens.spacing[gap]};
  
  @media (max-width: ${breakpoints.tablet}) {
    grid-template-columns: repeat(
      ${({ tabletColumns = 8 }) => tabletColumns},
      1fr
    );
  }
  
  @media (max-width: ${breakpoints.mobile}) {
    grid-template-columns: repeat(
      ${({ mobileColumns = 4 }) => mobileColumns},
      1fr
    );
  }
`;
```

### 3.2 Component Layout
```typescript
// Flexible layout components
export const Stack = styled.div<StackProps>`
  display: flex;
  flex-direction: ${({ direction = 'column' }) => direction};
  gap: ${({ spacing = 'md' }) => tokens.spacing[spacing]};
  align-items: ${({ align = 'stretch' }) => align};
  justify-content: ${({ justify = 'flex-start' }) => justify};
`;

export const Cluster = styled.div<ClusterProps>`
  display: flex;
  flex-wrap: wrap;
  gap: ${({ spacing = 'sm' }) => tokens.spacing[spacing]};
  align-items: ${({ align = 'center' }) => align};
  justify-content: ${({ justify = 'flex-start' }) => justify};
`;
```

## 4. Component Patterns

### 4.1 Form Patterns
```typescript
// Form component patterns
export const FormField = styled.div`
  display: flex;
  flex-direction: column;
  gap: ${tokens.spacing.xs};
  
  label {
    font-size: ${typeScale.small};
    font-weight: ${fontWeights.medium};
    color: ${colors.text.secondary};
  }
  
  input, select, textarea {
    padding: ${tokens.spacing.sm} ${tokens.spacing.md};
    border: 1px solid ${colors.border.default};
    border-radius: ${tokens.borderRadius.md};
    transition: all 0.2s ease;
    
    &:focus {
      border-color: ${colors.primary.base};
      box-shadow: 0 0 0 3px ${colors.primary.light};
    }
    
    &:invalid {
      border-color: ${colors.error.base};
    }
  }
`;
```

### 4.2 Card Patterns
```typescript
// Card component patterns
export const ContentCard = styled.div<CardProps>`
  background: ${colors.background.card};
  border-radius: ${tokens.borderRadius.lg};
  overflow: hidden;
  transition: transform 0.3s ${easings.easeOutCubic};
  
  // Card header
  header {
    padding: ${tokens.spacing.md};
    border-bottom: 1px solid ${colors.border.light};
  }
  
  // Card content
  .content {
    padding: ${tokens.spacing.lg};
  }
  
  // Card footer
  footer {
    padding: ${tokens.spacing.md};
    border-top: 1px solid ${colors.border.light};
    background: ${colors.background.subtle};
  }
  
  // Interactive states
  &:hover {
    transform: translateY(-4px);
  }
`;
```

## 5. Navigation Patterns

### 5.1 Main Navigation
```typescript
// Main navigation pattern
export const MainNav = styled.nav`
  // Container
  .nav-container {
    display: flex;
    align-items: center;
    justify-content: space-between;
    height: 64px;
    padding: 0 ${tokens.spacing.lg};
  }
  
  // Navigation items
  .nav-items {
    display: flex;
    gap: ${tokens.spacing.md};
    
    @media (max-width: ${breakpoints.tablet}) {
      display: none;
    }
  }
  
  // Mobile menu
  .mobile-menu {
    display: none;
    
    @media (max-width: ${breakpoints.tablet}) {
      display: block;
    }
  }
`;
```

### 5.2 Page Navigation
```typescript
// Page navigation pattern
export const PageNav = styled.nav`
  // Breadcrumbs
  .breadcrumbs {
    display: flex;
    align-items: center;
    gap: ${tokens.spacing.sm};
    margin-bottom: ${tokens.spacing.lg};
    
    a {
      color: ${colors.text.secondary};
      text-decoration: none;
      
      &:hover {
        color: ${colors.primary.base};
      }
    }
  }
  
  // Section navigation
  .section-nav {
    position: sticky;
    top: ${tokens.spacing.xl};
    padding: ${tokens.spacing.md};
    background: ${colors.background.subtle};
    border-radius: ${tokens.borderRadius.md};
  }
`;
```

## 6. Feedback Patterns

### 6.1 Loading States
```typescript
// Loading patterns
export const LoadingStates = {
  // Skeleton loading
  Skeleton: styled.div`
    background: linear-gradient(
      90deg,
      ${colors.skeleton.base} 25%,
      ${colors.skeleton.highlight} 50%,
      ${colors.skeleton.base} 75%
    );
    background-size: 200% 100%;
    animation: shimmer 1.5s infinite;
  `,
  
  // Spinner
  Spinner: styled.div`
    width: 24px;
    height: 24px;
    border: 2px solid ${colors.primary.light};
    border-top-color: ${colors.primary.base};
    border-radius: 50%;
    animation: spin 0.75s linear infinite;
  `
};
```

### 6.2 Error States
```typescript
// Error patterns
export const ErrorStates = {
  // Error message
  Message: styled.div`
    padding: ${tokens.spacing.md};
    background: ${colors.error.light};
    border: 1px solid ${colors.error.base};
    border-radius: ${tokens.borderRadius.md};
    color: ${colors.error.dark};
  `,
  
  // Form field error
  FieldError: styled.span`
    font-size: ${typeScale.small};
    color: ${colors.error.base};
    margin-top: ${tokens.spacing.xs};
  `
};
```

## 7. Responsive Patterns

### 7.1 Container Queries
```typescript
// Container query patterns
export const ResponsiveContainer = styled.div`
  container-type: inline-size;
  
  @container (min-width: 600px) {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: ${tokens.spacing.lg};
  }
  
  @container (min-width: 900px) {
    grid-template-columns: repeat(3, 1fr);
  }
`;
```

### 7.2 Adaptive Components
```typescript
// Adaptive component patterns
export const AdaptiveLayout = styled.div`
  // Base layout
  display: flex;
  flex-direction: column;
  gap: ${tokens.spacing.md};
  
  // Tablet layout
  @media (min-width: ${breakpoints.tablet}) {
    flex-direction: row;
    align-items: flex-start;
    
    .sidebar {
      width: 300px;
      flex-shrink: 0;
    }
    
    .content {
      flex-grow: 1;
    }
  }
  
  // Desktop layout
  @media (min-width: ${breakpoints.desktop}) {
    .sidebar {
      width: 360px;
    }
  }
`;
```

## 8. Implementation Examples

### 8.1 Analysis Form
```typescript
// Analysis form implementation
export const AnalysisForm = () => {
  return (
    <FormContainer>
      <Stack spacing="lg">
        <FormField>
          <label>Năm sinh</label>
          <input type="number" min="1900" max="2100" />
        </FormField>
        
        <FormField>
          <label>Giới tính</label>
          <select>
            <option value="male">Nam</option>
            <option value="female">Nữ</option>
          </select>
        </FormField>
        
        <DirectionSelector />
        
        <Button variant="primary">
          Phân tích
        </Button>
      </Stack>
    </FormContainer>
  );
};
```

### 8.2 Results Display
```typescript
// Results display implementation
export const AnalysisResults = () => {
  return (
    <ResultsContainer>
      <Grid columns={12} gap="lg">
        <GridItem span={8}>
          <ContentCard>
            <header>
              <h2>Kết quả phân tích</h2>
            </header>
            
            <div className="content">
              <Stack spacing="lg">
                <KuaNumberDisplay />
                <DirectionsChart />
                <RecommendationsList />
              </Stack>
            </div>
          </ContentCard>
        </GridItem>
        
        <GridItem span={4}>
          <Stack spacing="md">
            <ShareCard />
            <SaveAnalysisCard />
          </Stack>
        </GridItem>
      </Grid>
    </ResultsContainer>
  );
}; 