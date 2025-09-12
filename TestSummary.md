# Test Summary

**Table of Contents**

- [Domain: Documents](#domain-documents)
- [Domain: ECO](#domain-eco)
- [Domain: Part](#domain-part)
- [Business: OOTB](#business-ootb)
- [Category: Core](#category-core)
- [Core: Login](#core-login)
- [Document: Create](#document-create)
- [ECO: Create](#eco-create)
- [Part: Create](#part-create)
- [Part: Release](#part-release)
- [SmokeTest: 1](#smoketest-1)
- [SmokeTest: 2](#smoketest-2)
- [No Trait](#no-trait)

## Domain: Documents

- `Admin_can_create_Document`
- `Admin_can_find_a_Document`
- `Admin_can_not_create_Document_without_an_item_number`

## Domain: ECO

- `Admin_can_create_an_ECO`
- `Admin_can_find_an_ECO`
- `Admin_can_not_create_ECO_without_a_title`

## Domain: Part

- `Admin_can_create_a_Part`
- `Admin_can_find_a_Part`
- `CM_can_create_a_new_Revision_of_a_Released_Part`
- `CM_can_delete_a_new_Part`
- `CM_can_not_delete_a_Released_Part`
- `CM_can_not_edit_a_Released_Part`
- `User_can_not_edit_a_part_locked_by_another_user`
- `Users_can_manually_Release_Part`

## Business: OOTB

- `Admin_can_create_an_ECO`
- `Admin_can_create_Document`
- `Admin_can_not_create_Document_without_an_item_number`
- `Admin_can_not_create_ECO_without_a_title`
- `CM_can_create_a_new_Revision_of_a_Released_Part`
- `CM_can_delete_a_new_Part`
- `CM_can_not_delete_a_Released_Part`
- `CM_can_not_edit_a_Released_Part`
- `User_can_not_edit_a_part_locked_by_another_user`

## Category: Core

- `Admin_can_find_a_Document`
- `Admin_can_find_a_Part`
- `Admin_can_find_an_ECO`
- `LoginWithFixture_ShouldHaveALoggedInUser`

## Core: Login

- `LoginWithFixture_ShouldHaveALoggedInUser`

## Document: Create

- `Admin_can_create_Document`
- `Admin_can_not_create_Document_without_an_item_number`

## ECO: Create

- `Admin_can_create_an_ECO`
- `Admin_can_not_create_ECO_without_a_title`

## Part: Create

- `Admin_can_create_a_Part`

## Part: Release

- `Users_can_manually_Release_Part`

## SmokeTest: 1

- `Admin_can_find_a_Document`
- `Admin_can_find_a_Part`
- `Admin_can_find_an_ECO`

## SmokeTest: 2

- `Admin_can_add_a_File_to_vault`

## No Trait

- `CM_can_Release_an_Item_via_ECO`
- `TestLab`

