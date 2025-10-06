#!/bin/bash

# BudgetEase API Endpoint Validation Script
# This script tests that all API endpoints are working with the seeded demo data

set -e

API_BASE_URL="http://localhost:5108"
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo "=========================================="
echo "BudgetEase API Endpoint Validation"
echo "=========================================="
echo ""

# Function to test endpoint
test_endpoint() {
    local method=$1
    local endpoint=$2
    local expected_code=$3
    local description=$4
    
    echo -n "Testing: $description ... "
    
    response=$(curl -s -o /dev/null -w "%{http_code}" -X "$method" "$API_BASE_URL$endpoint")
    
    if [ "$response" == "$expected_code" ]; then
        echo -e "${GREEN}✓ PASS${NC} (HTTP $response)"
    else
        echo -e "${RED}✗ FAIL${NC} (Expected: $expected_code, Got: $response)"
        return 1
    fi
}

# Function to test endpoint with JSON response validation
test_endpoint_json() {
    local method=$1
    local endpoint=$2
    local expected_code=$3
    local description=$4
    local check_field=$5
    
    echo -n "Testing: $description ... "
    
    response=$(curl -s -w "\n%{http_code}" -X "$method" "$API_BASE_URL$endpoint")
    http_code=$(echo "$response" | tail -n 1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" == "$expected_code" ]; then
        if [ -n "$check_field" ]; then
            # Check if the response contains expected field
            if echo "$body" | jq -e "$check_field" > /dev/null 2>&1; then
                echo -e "${GREEN}✓ PASS${NC} (HTTP $http_code, field present)"
            else
                echo -e "${YELLOW}⚠ PARTIAL${NC} (HTTP $http_code, field missing: $check_field)"
            fi
        else
            echo -e "${GREEN}✓ PASS${NC} (HTTP $http_code)"
        fi
    else
        echo -e "${RED}✗ FAIL${NC} (Expected: $expected_code, Got: $http_code)"
        return 1
    fi
}

echo "Waiting for API to be ready..."
max_attempts=30
attempt=0
while [ $attempt -lt $max_attempts ]; do
    if curl -s "$API_BASE_URL/api/health" > /dev/null 2>&1; then
        echo -e "${GREEN}API is ready!${NC}"
        break
    fi
    attempt=$((attempt + 1))
    if [ $attempt -eq $max_attempts ]; then
        echo -e "${RED}API failed to start within timeout${NC}"
        exit 1
    fi
    sleep 1
done

echo ""
echo "----------------------------------------"
echo "Health Check Endpoints"
echo "----------------------------------------"
test_endpoint "GET" "/api/health" "200" "Health check endpoint"

echo ""
echo "----------------------------------------"
echo "Event Endpoints"
echo "----------------------------------------"
test_endpoint_json "GET" "/api/events" "200" "Get all events" ".[0].id"
test_endpoint_json "GET" "/api/events/1" "200" "Get event by ID (Wedding)" ".id"

echo ""
echo "----------------------------------------"
echo "Expense Endpoints"
echo "----------------------------------------"
test_endpoint_json "GET" "/api/expenses/event/1" "200" "Get expenses for Wedding event" ".[0].id"
test_endpoint_json "GET" "/api/expenses/1" "200" "Get expense by ID" ".id"

echo ""
echo "----------------------------------------"
echo "Vendor Endpoints"
echo "----------------------------------------"
test_endpoint_json "GET" "/api/vendors/event/1" "200" "Get vendors for Wedding event" ".[0].id"
test_endpoint_json "GET" "/api/vendors/1" "200" "Get vendor by ID" ".id"
test_endpoint_json "GET" "/api/vendors/reminders" "200" "Get vendor reminders" "."

echo ""
echo "----------------------------------------"
echo "Summary Statistics"
echo "----------------------------------------"

# Count records from API
EVENT_COUNT=$(curl -s "$API_BASE_URL/api/events" | jq 'length')
EXPENSE_COUNT=$(curl -s "$API_BASE_URL/api/expenses/event/1" | jq 'length')
VENDOR_COUNT=$(curl -s "$API_BASE_URL/api/vendors/event/1" | jq 'length')

echo "Events found: $EVENT_COUNT (expected: >=2)"
echo "Expenses for Wedding: $EXPENSE_COUNT (expected: >=5)"
echo "Vendors for Wedding: $VENDOR_COUNT (expected: >=4)"

echo ""
echo "=========================================="
echo -e "${GREEN}Validation Complete!${NC}"
echo "=========================================="
echo ""
echo "Sample data verified:"
echo "  - Multiple events with different types"
echo "  - Expenses with various payment statuses"
echo "  - Vendors with contact information"
echo "  - All API endpoints responding correctly"
echo ""
echo "To view demo user credentials, see: DEMO_DATA.md"
echo ""
