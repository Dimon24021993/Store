<template>
  <v-card hover>
    <v-layout row>
      <v-flex>
        <v-img :src="item.pictures.find(x => x.type == 2).href" width="100px">
        </v-img>
      </v-flex>
      <v-flex>
        <v-layout column fill-height>
          <v-flex>
            <v-layout row wrap>
              <v-flex xs12 class="body-2">
                <router-link :to="`/item/${item.id}`">
                  {{ item.originalName }}
                </router-link>
              </v-flex>
              <v-flex>
                {{ item.descriptions[0].descriptionText }}
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-card>
</template>
<script>
export default {
  data() {
    return {};
  },
  components: {},
  props: {
    item: { type: Object }
  }
};
</script>
