<?php
include_once("../arc2/ARC2.php");

$config = array(
  /* db */
  'db_host' => '????????????????????????',
  'db_name' => '???????????', /* dev */
  'db_user' => '????????????',
  'db_pwd' => '????????????',
  /* store */
  'store_name' => 'arc_tests',
   /* endpoint */
  'endpoint_features' => array(
    'select', 'construct', 'ask', 'describe', 
    'load', 'insert', 'delete', 
    'dump' /* dump is a special command for streaming SPOG export */
  ),
  'endpoint_timeout' => 60, /* not implemented in ARC2 preview */
  'endpoint_read_key' => '', /* optional */
  'endpoint_write_key' => '??????????', /* optional, but without one, everyone can write! */
  'endpoint_max_limit' => 250, /* optional */
);

/* instantiation */
$ep = ARC2::getStoreEndpoint($config);

if (!$ep->isSetUp()) {
  $ep->setUp(); /* create MySQL tables */
}

/* request handling */
$ep->go();

?>
