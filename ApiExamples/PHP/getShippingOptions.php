<?php

require_once "C:/php/php-5.4.4-nts-Win32-VC9-x86/include/rest_client.php";

//Sandbox
$root_url = "https://connect.sandbox.mimeo.com/2012/02/";
$user_name = "user@mimeo.com";
$password = "passwd!";

$url = $root_url . "Orders/GetOrderRequest";

echo "ENDPOINT: " . $url . "\r\n";

$restOut = RestClient::call("GET", $url, null, $user_name, $password, null);
$output = $restOut->getResponse();

$xml = new SimpleXMLElement($output);

$Items = $xml->LineItems;
$Items->AddLineItemRequest[0]->StoreItemReference->Id = "0b33e3d1-74b0-4e21-a5e1-5b878d28e837";
$Items->AddLineItemRequest[0]->Name = "Doc One";

$Items->AddLineItemRequest[1]->Name = "Doc Two";
$Items->AddLineItemRequest[1]->StoreItemReference->Id = "0b33e3d1-74b0-4e21-a5e1-5b878d28e837";
$Items->AddLineItemRequest[1]->Quantity = "1";


foreach ($Items->AddLineItemRequest as $AddLineItemRequest) {

    echo "Id: " . $AddLineItemRequest->StoreItemReference->Id . "\r\n";
    echo "Qty: " . $AddLineItemRequest->Quantity . "\r\n";
    echo "Name: " . $AddLineItemRequest->Name . "\r\n";
}

$xml->PaymentMethod->attributes('i', TRUE)->type = "UserCreditLimitPaymentMethod";

$Address = $xml->Recipients[0]->AddRecipientRequest->Address;
$Address->FirstName = "Jesus";
$Address->LastName = "Moncada";
$Address->Street = "10226 Signal Hill RD";
$Address->City = "Austin";
$Address->StateOrProvince = "TX";
$Address->Country = "US";
$Address->PostalCode = "78737";
$Address->TelephoneNumber = "555-555-5555";
$Address->IsResidential = "false";


$sendXML = $xml->asXML();


$url = $root_url . "Orders/GetShippingOptions";

echo "ENDPOINT: " . $url . "\r\n";

echo "XML before POST\r\n";
echo $xml->asXML() . "\r\n";

$restPostOut = RestClient::call("POST", $url, $sendXML, $user_name, $password, "application/xml");
$output = $restPostOut->getResponse();

echo "OUT: " . $output . "\r\n";

$xml = new SimpleXMLElement($output);
foreach ($xml->AvailableDeliveryOptionsPerRecipient->RecipientAvailableDeliveryOptions->DeliveryOptionQuotes->DeliveryOptionQuote as $ShippingOption) 
{	
	echo "\r\nID: " . $ShippingOption->ShippingMethodDetail->Id . "\r\n";	
	echo "Name: " . $ShippingOption->ShippingMethodDetail->Name . "\r\n";	
	echo "Charge: " . $ShippingOption->DeliveryCharge . "\r\n";	
	echo "DeliveryDate: " . $ShippingOption->DeliveryCommitmentDate . "\r\n\r\n";	
}



?>