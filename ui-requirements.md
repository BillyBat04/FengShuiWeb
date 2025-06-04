# UI/UX Requirements - FengShuiWeb Frontend

## 1. Nguyên Tắc Thiết Kế

### 1.1 Màu Sắc
```css
:root {
  /* Primary Colors */
  --primary-50: #f0fdf4;
  --primary-100: #dcfce7;
  --primary-500: #22c55e;
  --primary-600: #16a34a;
  --primary-700: #15803d;
  
  /* Neutral Colors */
  --neutral-50: #f8fafc;
  --neutral-100: #f1f5f9;
  --neutral-700: #334155;
  --neutral-900: #0f172a;
  
  /* Accent Colors */
  --accent-amber: #f59e0b;
  --accent-red: #ef4444;
  --accent-blue: #3b82f6;
}
```

### 1.2 Typography
```css
:root {
  /* Font Families */
  --font-primary: 'Inter', sans-serif;
  --font-secondary: 'Playfair Display', serif;
  
  /* Font Sizes */
  --text-xs: 0.75rem;
  --text-sm: 0.875rem;
  --text-base: 1rem;
  --text-lg: 1.125rem;
  --text-xl: 1.25rem;
  --text-2xl: 1.5rem;
  --text-3xl: 1.875rem;
  --text-4xl: 2.25rem;
}
```

### 1.3 Spacing & Layout
```css
:root {
  /* Spacing Scale */
  --spacing-1: 0.25rem;
  --spacing-2: 0.5rem;
  --spacing-4: 1rem;
  --spacing-6: 1.5rem;
  --spacing-8: 2rem;
  --spacing-12: 3rem;
  --spacing-16: 4rem;
  
  /* Container Widths */
  --container-sm: 640px;
  --container-md: 768px;
  --container-lg: 1024px;
  --container-xl: 1280px;
}
```

## 2. Components Design

### 2.1 Navigation
- Sticky header với gradient background
- Smooth dropdown menus
- Mobile-responsive hamburger menu
- Active state indicators
- Search functionality

### 2.2 Phong Thủy Analysis Form
- Multi-step form với progress indicator
- Interactive direction selector
- Date picker với lunar calendar
- Real-time validation
- Loading states và animations

### 2.3 Results Display
- Card-based layout
- Interactive charts và diagrams
- Color-coded recommendations
- Expandable sections
- Share functionality

### 2.4 Comparison View
- Side-by-side comparison
- Highlight differences
- Synchronized scrolling
- Filter và sort options

## 3. Animations & Interactions

### 3.1 Page Transitions
```typescript
// Framer Motion Variants
export const pageTransition = {
  initial: { opacity: 0, y: 20 },
  animate: { opacity: 1, y: 0 },
  exit: { opacity: 0, y: -20 },
  transition: { duration: 0.4, ease: "easeInOut" }
};
```

### 3.2 Micro-interactions
- Button hover/active states
- Input focus effects
- Loading spinners
- Success/error animations
- Tooltip animations

### 3.3 Scroll Effects
- Parallax scrolling
- Reveal on scroll
- Smooth scroll to sections
- Infinite scroll for results

## 4. Responsive Design

### 4.1 Breakpoints
```typescript
export const breakpoints = {
  sm: '640px',
  md: '768px',
  lg: '1024px',
  xl: '1280px',
  '2xl': '1536px'
};
```

### 4.2 Mobile First
- Stack layouts on mobile
- Touch-friendly interactions
- Simplified menus
- Optimized images
- Reduced animations

### 4.3 Tablet/Desktop
- Multi-column layouts
- Hover states
- Enhanced animations
- Advanced features
- Keyboard shortcuts

## 5. Performance Optimization

### 5.1 Loading States
- Skeleton screens
- Progressive loading
- Lazy image loading
- Prefetching data
- Caching strategies

### 5.2 Code Optimization
- Code splitting
- Tree shaking
- Bundle size optimization
- Memory management
- Runtime performance

## 6. Accessibility

### 6.1 WCAG Guidelines
- Color contrast
- Keyboard navigation
- Screen reader support
- Focus management
- ARIA labels

### 6.2 Inclusive Design
- Font scaling
- High contrast mode
- Reduced motion
- Alternative text
- Error handling

## 7. User Experience Enhancement

### 7.1 Error Prevention
- Clear validation messages
- Confirmation dialogs
- Undo functionality
- Auto-save
- Session recovery

### 7.2 User Feedback
- Toast notifications
- Progress indicators
- Success messages
- Error handling
- Loading states

### 7.3 Navigation
- Breadcrumbs
- Back buttons
- Search functionality
- Recent items
- Bookmarks

## 8. Advanced Features

### 8.1 Data Visualization
- Interactive charts
- Custom diagrams
- 3D models
- Animations
- Export options

### 8.2 Social Features
- Share buttons
- Comments
- Ratings
- User profiles
- Activity feed

### 8.3 Customization
- Theme switching
- Layout options
- Language selection
- Font sizing
- Notification preferences 