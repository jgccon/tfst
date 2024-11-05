terraform {
  source = "../../modules/storage"
}

include {
  path = find_in_parent_folders()
}

inputs = {
  storage_account_name = "tfstprodstorage"
}
