<?php
error_reporting(E_ALL);
ini_set('display_errors', '1');

require_once "rest_client.php";

//Sandbox
$root_url = "https://connect.sandbox.mimeo.com/2012/02/";
$user_name = "user@email.com";
$password = "testme@";

$url = $root_url . "StorageService/";
$file_name_with_full_path = realpath('./SSO_SAML_DevGuide.pdf');
$data = array('@'.$file_name_with_full_path);
$restOut = RestClient::call("POST", $url, $data, $user_name, $password);
$output = $restOut->getResponse();
$xml = new SimpleXMLElement($output);
$fileId = (string)$xml->Files->File->FileId;

// HARDCODE Template Id
$DocumentTemplateId = "e1546361-8dcb-4afb-b794-88af9603b3f3";
$url = $root_url . "StorageService/NewDocument?DocumentTemplateId=".$DocumentTemplateId;
$restOut = RestClient::call("GET", $url, null, $user_name, $password, null);
$output = $restOut->getResponse();
$xml = new SimpleXMLElement($output);

$xml->Name = "SSO_SAML_DevGuide";
$Section = $xml->Product->Content->DocumentSection[0];
$Section->Source = $fileId;
$Section->Range[0] = "[1,2]";
$sendXML = $xml->asXML();
$url = $root_url . "StorageService/Document/BOM%20Documents";

$restPostOut = RestClient::call("POST", $url, $sendXML, $user_name, $password, "application/xml");
$output = $restPostOut->getResponse();
$xml = new SimpleXMLElement($output);

print('<pre>');
print_r($output);
print_r($xml);
?>

