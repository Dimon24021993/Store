<template>
  <v-container grid-list-xl>
    <v-layout row wrap>
      <v-flex v-for="(item, i) in items" :key="i" xs6 sm4 md3>
        <template v-if="items && items.length">
          <item-card :item="item"> </item-card>
        </template>
      </v-flex>
      <v-flex>
        <v-pagination
          v-model="currentPage"
          :length="pagination.pages"
          @input="pageChange"
        ></v-pagination>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import { mapState, mapActions } from "vuex";
import itemCard from "./ItemTileCard";

export default {
  data() {
    return {
      currentPage: 1
    };
  },
  components: {
    itemCard
  },
  created() {
    this.loadItems({
      data: {
        size: this.pagination.size,
        page: this.$route.params.page || this.pagination.page
      }
    });
    this.currentPage = +this.$route.params.page || 1;
  },
  computed: {
    itemsWithImage() {
      return this.items
        .map(x => {
          var banners = x.pictures.filter(x => x.type == 2);
          if (banners.length) x.image = banners[0].href;
          return x;
        })
        .filter(x => x && !!x.image);
    },
    options() {
      return {
        spaceBetween: 0,
        loop: true,
        loopedSlides: this.pagination.size, //looped slides should be the same
        navigation: {
          nextEl: ".swiper-button-next",
          prevEl: ".swiper-button-prev"
        }
      };
    },
    ...mapState({
      pagination: state => state.items.pagination,
      items: state => state.items.pagination.entities
    })
  },
  methods: {
    pageChange($event) {
      this.$router.push("/page/" + $event);
      this.loadItems({ data: { size: this.pagination.size, page: $event } });
    },
    ...mapActions({
      loadItems: "items/getItems"
    })
  }
};
</script>

