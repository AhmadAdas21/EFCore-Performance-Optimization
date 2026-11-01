
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
