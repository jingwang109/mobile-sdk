<?
include "rsa.php";
$content="{\"timeStamp\":1363848203377,\"orderId\":100001472,\"money\":4000,\"chargeType\":\"BALANCEPAY\",\"appKeyId\":100000000,\"buyerId\":1,\"cardNo\":null}";
$sign="VwnhaP9gAbDD2Msl3bFnvsJfgz3NOAqM/JVexl1myHfsrHX3cRrFXz86cNO+oNYWBBM7m/5ZdtHRpSArZWFuZHysKfirO3BynUaIYSAiD2J1Xio5q9+Yr83cI/ESyemVAt7lK4lMW3ReSwmAcOs0kDZLAxVIb++EPy0y2NpH4kI=";
$rsa=new Rsa;
$check=$rsa->verify($content,$sign);
echo "true=".$check."\n";
$check=$rsa->check($content,$sign);
echo  ""+true ;
$check=$rsa->verify("testError",$sign);
echo "false=".$check."\n";
?>

