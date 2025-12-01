# Building Entry Registration System



## Project Overview

A full-stack web application for visitor check-in at building entrances. Built with Angular 20 frontend and .NET 9/10 backend REST API.


## Features

### Implemented Features (Visitor Flow)

1. **Entrance ID Entry**
   - QR code scanning support
   - Manual entrance ID input
   - Entrance ID validation via API
2. **Personal Information**
   - Name, email, and company name collection
   - Form validation
   - Session-based data persistence
3. **Team Selection**
   - Dynamic team loading from API
   - Dropdown selection
   - Pre-seeded team data
4. **Rules Acceptance**
   - Required checkbox for terms & conditions
   - Validation before proceeding
5. **Review & Check-in**
   - Summary of all entered information
   - Final check-in submission
   - Success screen with confirmation details



### Technical Features

- **Mobile-first responsive design**
- **Angular 20** with standalone components
- **.NET 9/10 REST API** with OpenAPI 3.0 specification
- **In-memory storage** (for assignment purposes)
- **Session management** using localStorage
- **Form validation** with Reactive Forms
- **Routing with lazy loading**
- **TypeScript interfaces** for type safety



## Architecture

### Frontend Structure

text

```
src/app/
├── components/
│   ├── entrance-id/          # Entrance ID screen
│   ├── personal-info/        # Personal information form
│   ├── select-team/          # Team selection
│   ├── accept-rules/         # Rules acceptance
│   ├── review/               # Review screen
│   └── success/              # Success confirmation
├── services/
│   ├── api.service.ts        # HTTP API communication
│   └── session.service.ts    # Session management
├── models/
│   ├── visitor.model.ts      # Visitor interface
│   ├── team.model.ts         # Team interface
│   └── session.model.ts      # Session interface
└── app.routes.ts             # Application routing
```



### Backend API Endpoints

- `POST /api/v1.0/Entrance/validate` - Validate entrance ID
- `GET /api/v1.0/Teams` - Get teams list
- `POST /api/v1.0/Visitor/save-personal-info` - Save personal info
- `POST /api/v1.0/Visitor/select-team` - Select team
- `POST /api/v1.0/Visitor/accept-rules` - Accept rules
- `POST /api/v1.0/Visitor/checkin` - Final check-in
- `GET /api/v1.0/Visitor/{sessionId}/review` - Get review data
- `GET /api/v1.0/Visitor/{id}` - Get visitor details



## Setup & Installation

### Prerequisites

- Node.js 18+ and npm
- .NET 9 SDK
- Angular CLI 20

### Backend Setup (.NET API)

bash

```
cd Backend
dotnet restore
dotnet build
dotnet run
```



API will be available at:

- HTTP: [http://localhost:5000](http://localhost:5000/)
- HTTPS: [https://localhost:5001](http://localhost:5001/)
- Swagger: http://localhost:5000/swagger

### Frontend Setup (Angular)

bash

```
cd FrontEnd
npm install
ng serve --open
```



Application will open at: [http://localhost:4200](http://localhost:4200/)

### Proxy Configuration (for CORS)

Create `proxy.conf.json`:

json

```
{
  "/api": {
    "target": "http://localhost:5000",
    "secure": false,
    "changeOrigin": true
  }
}
```



Run with proxy:

bash

```
ng serve --open --proxy-config proxy.conf.json
```



## User Flow

1. **Entrance Screen**
   - Enter or scan Entrance ID
   - Validation against backend
2. **Personal Information**
   - Enter name, email, company
   - Form validation
3. **Team Selection**
   - Select from pre-loaded teams
   - Teams loaded from `/api/v1.0/Teams`
4. **Rules Acceptance**
   - Read and accept terms & conditions
   - Required checkbox validation
5. **Review**
   - Summary of all entered information
   - Confirm details before check-in
6. **Success**
   - Confirmation screen with check-in details
   - Option to start new check-in



## Business Rules

1. **Entrance-first Validation**: Valid entrance ID required before proceeding
2. **Required Fields**: All personal information fields mandatory
3. **Team Selection**: Visitor must select from available teams
4. **Rules Acceptance**: Mandatory acceptance of terms & conditions
5. **Data Storage**: All timestamps stored in UTC, displayed in local time



## Data Models

### Visitor

typescript

```
interface Visitor {
  id?: string;
  sessionId: string;
  entranceId: string;
  name: string;
  email: string;
  company: string;
  teamId: string;
  teamName?: string;
  rulesAccepted: boolean;
  checkInTime?: Date;
  checkOutTime?: Date;
}
```



### Team

typescript

```
interface Team {
  id: string;
  name: string;
  description?: string;
}
```



### Session

typescript

```
interface SessionData {
  sessionId: string;
  entranceId: string;
  currentStep: number;
  visitorData: Partial<Visitor>;
}
```



## Testing

### Backend Tests

bash

```
cd Backend
dotnet test
```

- Unit test for entrance ID validation
- Business rule enforcement tests
  

### Frontend Tests

bash

```
cd FrontEnd
ng test
```

- Component unit tests
- Service tests



## Planned Features (Not Implemented)

### Check-out Flow

- **Exit Entrance ID**: Separate ID for check-out
- **Check-out Recording**: Timestamp and duration calculation
- **Validation**: Verify visitor checked in before allowing check-out

### Admin Capabilities

- **Dashboard**: View today's visitors
- **Reporting**: Generate visitor reports
- **Code Management**: Entrance ID generation/management
- **Real-time Monitoring**: Current building occupancy

### Enhanced Features

- **QR Code Generation**: Dynamic QR codes for entrance
- **Email Notifications**: Check-in confirmations
- **Photo Capture**: Visitor photo for identification
- **Multi-language Support**
- **Accessibility Features**: Screen reader support
- **Offline Mode**: Local storage with sync



## API Documentation

Full OpenAPI 3.0 specification included in the project. Available at:

- Swagger UI: `http://localhost:5000/swagger`
- OpenAPI JSON: `http://localhost:5000/swagger/v1/swagger.json`



## Development Notes

### Key Decisions

1. **Standalone Components**: Angular 20 standalone components for better performance
2. **Session-based Flow**: localStorage for multi-step form persistence
3. **Mobile-first Design**: Responsive design for mobile devices
4. **In-memory Storage**: For assignment simplicity (production would use database)
5. **Type Safety**: Full TypeScript interfaces for API contracts


### Trade-offs

1. **Security**: No authentication for simplicity (production would require JWT)
2. **Persistence**: In-memory storage loses data on restart
3. **Validation**: Basic validation (production would need more robust validation)
4. **Error Handling**: Basic error handling for assignment scope


## Known Issues

1. **CORS Configuration**: May require proxy or backend CORS configuration
2. **Session Persistence**: localStorage based, may have limitations
3. **Form Validation**: Basic validation implemented



## Submission Notes

### What's Working

- Complete visitor check-in flow
- Mobile-responsive design
- API integration (with proper configuration)
- Session management
- Form validation
- Routing with step tracking

### What's Missing

- Check-out flow implementation
- Admin capabilities
- Database persistence
- Authentication/authorization
- Comprehensive error handling
- Unit tests for all components

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is developed with FREE license.

## 👤 Author

Building Entry Registration System
Contact: aykut@aykutaktas.net
Website: [https://www.aykutaktas.net](https://www.aykutaktas.net/)

------

**Note**: This is a demonstration project for a technical assignment. Production deployment would require additional security, persistence, and scalability considerations.