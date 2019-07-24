<template>
  <v-card hover height="100%" @click="$router.push(itemHref)" tile>
    <v-img :src="titleHref"></v-img>
    <v-card-title class="pa-2" height="100%">
      <div>
        <router-link :to="itemHref">
          {{ item.name }}
        </router-link>
      </div>
    </v-card-title>
  </v-card>
</template>
<script>
export default {
  data() {
    return {};
  },
  computed: {
    titleHref() {
      var a = this.item.pictures[0];
      return a ? a.href : "";
    },
    itemHref() {
      return `/item/${this.item.itemNo}`;
    }
  },
  props: {
    item: { type: Object }
  }
};
</script>
<style>
.rate {
  background: #53c70fd3;
}
</style>
