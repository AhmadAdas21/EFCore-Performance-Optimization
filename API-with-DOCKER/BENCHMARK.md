
# EF Core Performance Benchmark

## Environment
- .NET 8 / EF Core 8
- SQL Server 2022
- Docker Compose
- Database: EFCorePerformanceDb

## Seeded Data
- Customers: 10,000
- Products: 20,000
- Orders: 50,000
- OrderItems: 200,000

## Slow Orders Report (N+1)

Endpoint:
GET /api/orders/report

| Page Size | SQL Commands | Avg. Execution Time |
|-----------|--------------|---------------------|
| 15        | 16           | 26.38 ms            |
| 100       | 101          | 115.08 ms           |

## Methodology
- SQL commands counted using DbCommandInterceptor.
- Execution time measured using BenchmarkMiddleware.
- Measurements collected from local Docker environment.
- Clear warm-up outliers excluded.
- Results are preliminary and should be reproduced.

## Findings
The endpoint suffers from N+1 queries.

One query retrieves a page of Orders.
An additional query counts OrderItems for each Order.

Total SQL commands = 1 + Number of Orders.

## Optimization Goal
Replace the per-order CountAsync queries with
server-side projection and aggregation.

Target: Reduce the report to one SQL query per request.
## Optimized vs Slow — Final Comparison

Environment: Local Docker, SQL Server 2022, EF Core 8
Page Size: 100
Samples: 5 requests per implementation

| Metric | Slow | Optimized |
|--------|------|-----------|
| SQL Commands | 101 | 1 |
| Avg HTTP Time | 99.87 ms | 7.14 ms |
| Median HTTP Time | 86.60 ms | 6.72 ms |

Results:
- SQL command reduction: 99.01%
- Average response time reduction: 92.85%
- Speedup: approximately 14x

Methodology:
- Alternating requests to both endpoints.
- SQL commands verified using DbCommandInterceptor.
- HTTP timings measured with curl.
- Results are local benchmark measurements.
- Results are local benchmark measurements.

## Index Optimization — Recent Orders

### Problem
The query retrieved the latest 100 orders by scanning
50,000 rows and performing a TopN Sort.

### Solution
Created a composite covering nonclustered index:

CREATE INDEX IX_Orders_CreatedAtUtc_Id
ON Orders (CreatedAtUtc DESC, Id DESC)
INCLUDE (CustomerId, TotalCost);

### Benchmark Results

| Metric | Old Plan | Optimized Plan |
|---|---|---|
| Logical Reads | 457 | 2 |
| Rows Read | 50,000 | 100 |
| Sort Operator | TopN Sort | Eliminated |
| Average Elapsed Time | 52.4 ms | 0 ms reported |
| Median Elapsed Time | 52 ms | 0 ms reported |

### Findings
- Logical reads reduced by 99.56%.
- Rows read reduced by 99.8%.
- The new index supports ordered retrieval.
- The TopN Sort operator was eliminated.
- No Key Lookup was required.

### Methodology
- SQL Server 2022 running in Docker.
- Database: EFCoreIndexLab.
- 50,000 Orders and 200,000 OrderItems.
- 10 alternating executions after warm-up.
- Old plan reproduced using INDEX(PK_Orders).
- Optimized plan used automatic index selection.
- SQL Server reported optimized execution times
  as 0 ms due to timer resolution.
- Results measure SQL execution, not HTTP latency.