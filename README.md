
# Instant Wellness Kits

**Order Management System with Geolocation-Based Tax Calculation.**

## Challenges & Solutions

A brief description of the challenges our team faced during the development of this solution.

|**Challenge**|**Solution**|
|---|---|
|**Restrictions on Third-Party APIs**|Midway through development, we decided to phase out external services due to API usage restrictions. We transitioned to working with local, pre-processed datasets, ensuring total system autonomy.|
|**Handling Out-of-State Points**|To verify coordinates, we implemented a **GeoJSON MultiPolygon** covering all land and water boundaries of New York, allowing for precise regional identification.|
|**Tax Calculation for Remote Areas (Forests, Rivers, etc.)**|We implemented an algorithm that anchors the tax rate to the nearest available Zip Code. This ensures seamless financial calculations for any point in New York, regardless of existing postal infrastructure.|
|**Slow Import of Large Datasets**|The loading process was optimized by implementing caching mechanisms and parallel database writes, significantly reducing CSV file processing time.|
|**Zip Code Ambiguity (One code spanning two states)**|To avoid tax rate errors, the system uses combined identification. Calculations are based not only on the Zip Code but also on a State Identifier, accurately separating jurisdictions with different tax levels even within the same index.|

> **Note:** Since physical delivery to certain zones (e.g., dense forests or the middle of a river) may be logistically complex, actual transportation to such locations is a separate case requiring additional research at the courier service level. Our primary goal was to ensure the stability of mathematical calculations for any geographic position.
> 
> **Important:** The solution architecture allows for polygon editing. This enables the automatic exclusion of forests, parks, or water bodies during the validation stage where delivery is known to be impossible.



## Flexibility & Scalability

The application architecture is designed with future expansion in mind. The system can adapt to calculate tax rates not only for New York but for other US states as well.

**Technical Implementation of Scaling:**

- **Geospatial Independence:** The system uses the GeoJSON standard to store state and county boundaries in MongoDB. To add a new region, simply upload the corresponding coordinate array (MultiPolygon) to the geozone collection without changing the source code.
- **Unified Data Model:** The tax rates collection (`tax_rates`) is designed on a "key-value" principle, where each key is a unique identifier (Zip Code + County Code). This allows for easy importing of new datasets for any state via the existing API endpoint.

## Tech Stack

- **Frontend:** React + TypeScript + Vite + Ant Design
- **Backend:** C# .NET 10 Minimal API
- **Database:** MongoDB
- **Containerization:** Docker + Docker Compose

## Frontend Features

- **Order Table:** Featuring server-side pagination and sorting.
- **Filters:** Filter by date range and total amount range.
- **Order Creation:** Manual creation of orders using specific coordinates.
- **CSV Import:** Bulk upload of orders from a CSV file.

## How to Run

**Prerequisites:**

- Docker installed and running
- Git

**Step-by-step Setup:**

1. **Clone the Repository**
 
    ```
    git clone https://github.com/QDestTM/iw-kits.git
    cd iw-kits
    ```
 
1. **Prepare Database Data**
    
Download the archive (`.zip` or `.rar`) from the **Releases** tab and extract the `mongodb` folder into the project root. It contains the necessary data and configurations for the backend calculations.
    
2. **Build Docker Images**
    
    - **API Image:**
        
        `docker build -t iw-kits/api -f backend/src/IWKits.Api/Dockerfile .`
        
    - **Frontend Image:**
        
        `docker build -t iw-kits/web -f frontend/src/IWKits.Client/Dockerfile .`
        
3. **Launch Services**
 
    ```
    docker compose up -d
    ```
 

## Authorization Data

Use the following credentials to access the management panel or the database:

|**Role**|**Login**|**Password**|**Access Method**|
|---|---|---|---|
|**Admin**|`iwkits-admin`|`secretpass`|Web App (Frontend)|
|**Admin**|`admin`|`iwkits000adm000pass`|MongoDB Compass|
|**API Service**|`api_service`|`iwkits000api000pass`|MongoDB Compass|

### Services

|**Service**|**URL**|**Description**|
|---|---|---|
|**Frontend**|`http://localhost:25000`|React SPA (Nginx)|
|**Backend API**|`http://localhost:23000`|.NET Minimal API|
|**MongoDB**|`http://localhost:24000`|Database|
|**Swagger**|`http://localhost:23000/swagger`|API Documentation|

## API Overview

### User Registration

`POST /api/v1/auth/register`

```json
{
  "username": "your_username",
  "password": "your_password"
}
```

### User Login

`POST /api/v1/auth/login`


```json
{
  "username": "your_username",
  "password": "your_password"
}
```

### Create Order

`POST /api/v1/orders`


```json
{
  "latitude": 40.7580,
  "longitude": -73.9855,
  "subtotal": 99.99
}
```

### Get Orders (Filtering, Sorting, Pagination)

`GET /api/v1/orders?page=1&page_size=24&sort_by=timestamp&descending=true`

| **Parameter**      | **Type** | **Description**                                              |
| ------------------ | -------- | ------------------------------------------------------------ |
| `page`             | int      | Page number (Default: 1)                                     |
| `page_size`        | int      | Items per page (Default: 24, Max: 128)                       |
| `sort_by`          | string   | Field: `timestamp`, `subtotal`, `total_amount`, `tax_amount` |
| `descending`       | bool     | Sorting direction, `true` for descending                     |
| `from_date`        | ISO date | Filter: orders after this date                               |
| `to_date`          | ISO date | Filter: orders before this date                              |
| `min_total_amount` | decimal  | Filter: minimum total amount                                 |
| `max_total_amount` | decimal  | Filter: maximum total amount                                 |

### Import Orders (CSV)

`POST /api/v1/orders/import`

**Content-Type:** `multipart/form-data`

**CSV Format:**

```
id,latitude,longitude,subtotal,timestamp
1001,40.7580,-73.9855,99.99,2026-01-15T10:30:00Z
```
