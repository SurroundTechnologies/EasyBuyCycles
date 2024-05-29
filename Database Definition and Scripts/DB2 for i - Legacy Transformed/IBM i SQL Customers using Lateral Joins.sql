SET SCHEMA EASYBUYDEM;
--SET SCHEMA EASYBUYDEV;
--SET SCHEMA EASYBUYMST;
--SET SCHEMA EASYBUYTRL;
--SET SCHEMA LOOKDEMO;

SELECT Customer.YD1CIID AS "Internal ID"
		,Customer.YD1CPTID AS "Parent Internal ID"
		,RTRIM(Customer.YD1CPTRL) AS "Parent Relationship"
		,RTRIM(Customer.YD1CNM) AS "Name"
		,RTRIM(Customer.YD1CNMLG) AS "Legal Name"
		,RTRIM(Customer.YD1CCNLN) AS "Contact Last Name"
		,RTRIM(Customer.YD1CCNFN) AS "Contact First Name"
		,RTRIM(Customer.YD1CCNMN) AS "Contact Middle Name"
		,RTRIM(Customer.YD1CCNNN) AS "Contact Nick Name"
		,RTRIM(Customer.YD1CBLA1) AS "Billing Address 1"
		,RTRIM(Customer.YD1CBLA2) AS "Billing Address 2"
		,RTRIM(Customer.YD1CBLA3) AS "Billing Address 3"
		,RTRIM(Customer.YD1CBLPC) AS "Billing Postal Code"
		,RTRIM(Customer.YD1CBLCY) AS "Billing Country"
		,RTRIM(Customer.YD1CTL) AS "Telephone"
		,RTRIM(Customer.YD1CEM) AS "Email"
		,RTRIM(Customer.YD1CM1) AS "Memo"
		,Customer.YD1CPRPT AS "Purchase Points"

-- Calculated Columns
		,RTRIM(Customer.YD1CCNFN) 
		   CONCAT CASE RTRIM(Customer.YD1CCNMN) WHEN '' THEN '' ELSE SPACE(1) CONCAT RTRIM(Customer.YD1CCNMN) END
		   CONCAT SPACE(1) CONCAT RTRIM(Customer.YD1CCNLN)
		   CONCAT CASE RTRIM(Customer.YD1CCNNN) WHEN '' THEN '' ELSE ' "' CONCAT RTRIM(Customer.YD1CCNNN) CONCAT '"' END
		   AS "Contact Full Name"
		,RTRIM(Customer.YD1CBLA1)
		   CONCAT CASE RTRIM(Customer.YD1CBLA2) WHEN '' THEN '' ELSE ', ' CONCAT RTRIM(Customer.YD1CBLA2) END
		   CONCAT CASE RTRIM(Customer.YD1CBLA3) WHEN '' THEN '' ELSE ', ' CONCAT RTRIM(Customer.YD1CBLA3) END
		   CONCAT CASE RTRIM(Customer.YD1CBLPC) WHEN '' THEN '' ELSE ' ' CONCAT RTRIM(Customer.YD1CBLPC) END
		   CONCAT CASE RTRIM(Customer.YD1CBLCY) WHEN '' THEN '' ELSE ', ' CONCAT RTRIM(Customer.YD1CBLCY) END
		   AS "Billing Address Line"
		,RTRIM(Customer.YD1CBLA1)
		   CONCAT CASE RTRIM(Customer.YD1CBLA2) WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM(Customer.YD1CBLA2) END
		   CONCAT CASE RTRIM(Customer.YD1CBLA3) WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM(Customer.YD1CBLA3) END
		   CONCAT CASE RTRIM(Customer.YD1CBLPC) WHEN '' THEN '' ELSE ' ' CONCAT RTRIM(Customer.YD1CBLPC) END
		   CONCAT CASE RTRIM(Customer.YD1CBLCY) WHEN '' THEN '' ELSE CHR(13) CONCAT RTRIM(Customer.YD1CBLCY) END
		   AS "Billing Address Block"
		,CASE Customer.YD1CPTID WHEN 0 THEN 0 ELSE 1 END AS "Is a Sub-Customer"
		
-- Joins to Customer
		,ParentCustomer.YD1CIID AS "Parent Customer Internal ID"
		,RTRIM(ParentCustomer.YD1CNM) AS "Parent Customer Name"
		
        ,SubCustomers.*

-- Join to Shipping Address
		,ShippingAddresses.*

-- Join to Order
		,Orders.*
		,LastOrder.*
		,OrderedProducts.*

-- Audit Stamps
--		,TIMESTAMP_FORMAT(CHAR(Customer.YD1CCRDT) CONCAT CHAR(Customer.YD1CCRTM),'YYYYMMDDHHMISS') AS "Create Date and Time"
		,Customer.YD1CCRDT AS "Create Date"
		,Customer.YD1CCRTM AS "Create Time"
		,Customer.YD1CCRUS AS "Create User"
		,Customer.YD1CCRJB AS "Create Job"
		,Customer.YD1CCRJN AS "Create Job Number"
--		,CASE WHEN Customer.YD1CLCDT=0 THEN TIMESTAMP_FORMAT('19000101000000','YYYYMMDDHHMISS') ELSE TIMESTAMP_FORMAT(CHAR(Customer.YD1CLCDT) CONCAT CHAR(Customer.YD1CLCTM),'YYYYMMDDHHMISS') END AS "Last Change Date and Time"
		,Customer.YD1CLCDT AS "Last Change Date"
		,Customer.YD1CLCTM AS "Last Change Time"
		,Customer.YD1CLCUS AS "Last Change User"
		,Customer.YD1CLCJB AS "Last Change Job"
		,Customer.YD1CLCJN AS "Last Change Job Number"

	FROM YD1C AS Customer
	LEFT JOIN YD1C AS ParentCustomer ON ParentCustomer.YD1CIID = Customer.YD1CPTID

	LEFT JOIN LATERAL (
		SELECT COUNT(*) AS "Sub-Customer Count"
				,CLOB(LISTAGG(RTRIM(YD1CNM),', ' ON OVERFLOW TRUNCATE '...' WITH COUNT) WITHIN GROUP(ORDER BY YD1CNM),5000) AS "Sub-Customer List"
			FROM YD1C
			WHERE YD1CPTID = Customer.YD1CIID
		) AS SubCustomers ON 'X'='X'

	LEFT JOIN LATERAL (
		SELECT COUNT(*) AS "Order Count"
				,SUM(CASE WHEN YD1OST IN('Open', 'New', 'Pending') THEN 1 ELSE 0 END) AS "Incomplete Order Count"
				,SUM(CASE WHEN OrderItems."Order Discount" > 0 THEN 1 ELSE 0 END) AS "Discounted Order Count"
				,DECIMAL(MAX(OrderItems."Order Subtotal"),11,2) AS "Highest Order Subtotal"
				,DECIMAL(MAX(OrderItems."Order Discount"),11,2) AS "Highest Discount"
				,DECIMAL(MAX(OrderItems."Order Total"),11,2) AS "Highest Order Total"
				,DECIMAL(AVG(OrderItems."Order Subtotal"),11,2) AS "Average Order Subtotal"
				,DECIMAL(AVG(OrderItems."Order Discount"),11,2) AS "Average Discount"
				,DECIMAL(AVG(OrderItems."Order Total"),11,2) AS "Average Order Total"
				,DECIMAL(MIN(OrderItems."Order Subtotal"),11,2) AS "Lowest Order Subtotal"
				,DECIMAL(MIN(OrderItems."Order Discount"),11,2) AS "Lowest Discount"
				,DECIMAL(MIN(OrderItems."Order Total"),11,2) AS "Lowest Order Total"
			FROM YD1O
			LEFT JOIN LATERAL (
				SELECT COUNT(*) AS "Order Item Count"
						,DECIMAL(SUM(YD1IQT * YD1IPRUN),11,2) AS "Order Subtotal"
						,DECIMAL(SUM(YD1IQT * YD1IPRUN * (YD1IDSPC*0.01)),11,2) AS "Order Discount"
						,DECIMAL(SUM(YD1IQT * YD1IPRUN * (1 - (YD1IDSPC*0.01))),11,2) AS "Order Total"
					FROM YD1I
					WHERE YD1I1OID = YD1OIID
				) AS OrderItems ON 'X'='X'
			WHERE YD1O1CID = Customer.YD1CIID
		) AS Orders ON 'X'='X'

	LEFT JOIN LATERAL (
		SELECT COUNT(*) AS "Shipping Address Count"
			FROM YD1S
			WHERE YD1S1CID = Customer.YD1CIID
		) AS ShippingAddresses ON 'X'='X' 

	LEFT JOIN LATERAL (
		SELECT YD1OIID AS "Last Order ID"
				,TIMESTAMP(YD1ODT,YD1OTM) AS "Last Order Date and Time"
				,YD1O1SID AS "Last Used Shipping Address ID"
				,YD1SNM AS "Last Used Shipping Address Name"
			FROM YD1O
			RIGHT JOIN YD1I ON YD1I1OID = YD1OIID
			LEFT JOIN YD1S ON YD1SIID = YD1O1SID
			WHERE YD1O1CID = Customer.YD1CIID
			ORDER BY YD1ODT DESC, YD1OTM DESC, YD1OIID DESC
			FETCH FIRST 1 ROW ONLY
		) AS LastOrder ON 'X'='X'

	LEFT JOIN LATERAL (
		SELECT COUNT(*) "Order Items Count"
				,COUNT(DISTINCT YD1I1PID) AS "Products Ordered Count"
			FROM YD1O
			RIGHT JOIN YD1I ON YD1I1OID = YD1OIID
			LEFT JOIN YD1P ON YD1PIID = YD1I1PID
			WHERE YD1O1CID = Customer.YD1CIID
		) AS OrderedProducts ON  'X'='X' 

	ORDER BY Customer.YD1CNM ASC

-- Use for testing querry to limit resultset
--	FETCH FIRST 5 ROW ONLY