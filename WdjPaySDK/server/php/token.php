<?
$tokenAPI="https://pay.wandoujia.com/api/uid/check";
$uid="8139480";
$token=urlencode("TdIKyj1Wqor3XrlamnLQcmCaqlHeClkPXg4OmPo3iBo=");
$url=$tokenAPI."?uid=".$uid."&token=".$token;
echo file_get_contents($url);
?>

